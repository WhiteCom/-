//-------------------------------------------------------------------
// 전처리기로 Astar 로직으로 할 지 Dijkstra 로직으로 할 지 결정
//-------------------------------------------------------------------

//#define Astar 
#define Dijkstra

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager instance;

    public GameObject ExceptionUI;

    public TileSc[] tileObjs;
    public Color[] initColors;

    public int max_X, max_Y;
    public int sX, sY; //start point
    public int eX, eY; //end point

    //
    //for Dijkstra
    //

    private int INF = 9999;
    private int MAX_V;

    public pair[][] adj;

    //=======================================================================
    // Unity LifeCycle
    //=======================================================================

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogWarning("이미 존재하는 GroundManager 입니다");
            Destroy(gameObject);
            return;
        }

        colorInit();
    }

    void Start()
    {

        init();
    }

    void Update()
    {

    }

    //=======================================================================
    // GroundManager Method
    //=======================================================================

    public void colorInit()
    {
        tileObjs = FindObjectsOfType<TileSc>();
        Sort();

        int len = tileObjs.Length;

        initColors = new Color[len];


        max_X = (int)tileObjs[len - 1].x;
        max_Y = (int)tileObjs[len - 1].y;

        MAX_V = (max_X + 1) * (max_Y + 1);

        for (int i = 0; i < MAX_V; i++)
        {
            //init Color
            initColors[i] = tileObjs[i].GetComponent<SpriteRenderer>().color;
        }
    }

    public void init()
    {
        tileObjs = FindObjectsOfType<TileSc>();
        Sort();

        int len = tileObjs.Length;

        max_X = (int)tileObjs[len - 1].x;
        max_Y = (int)tileObjs[len - 1].y;

        MAX_V = (max_X + 1) * (max_Y + 1);

        FindStartEnd();

#if Dijkstra
        DijkstraAdj();
#endif
    }

    private void FindStartEnd()
    {
        for (int i = 0; i < MAX_V; i++)
        {
            //find startP
            if (tileObjs[i].start)
            {
                sX = (int)tileObjs[i].transform.position.x;
                sY = (int)tileObjs[i].transform.position.y;
            }
            if (tileObjs[i].end)
            {
                eX = (int)tileObjs[i].transform.position.x;
                eY = (int)tileObjs[i].transform.position.y;
            }

        }
    }

    private void Sort() //TileSc 를 정렬
    {
        //Bubble Sort
        int len = tileObjs.Length;

        TileSc tmp;

        //Y sorting
        for (int i = 0; i < len - 1; i++)
        {
            for (int j = i + 1; j < len; j++)
            {
                if (tileObjs[i].y > tileObjs[j].y)
                {
                    tmp = tileObjs[i];
                    tileObjs[i] = tileObjs[j];
                    tileObjs[j] = tmp;
                }
            }
        }

        //X sorting
        for (int i = 0; i < len - 1; i++)
        {
            for (int j = i + 1; j < len; j++)
            {
                if (tileObjs[i].x > tileObjs[j].x && tileObjs[i].y == tileObjs[j].y)
                {
                    tmp = tileObjs[i];
                    tileObjs[i] = tileObjs[j];
                    tileObjs[j] = tmp;
                }
            }
        }
    }

    //=====================================================
    // Color Way
    //=====================================================

    public void ResetColor()
    {
        int len = tileObjs.Length;
        for (int i = 0; i < len; i++)
        {
            tileObjs[i].GetComponent<SpriteRenderer>().color = initColors[i];
        }
    }

    public void ChangeColor(Color newColor, int[] colorWay)
    {
        int len = colorWay.Length;
        for (int i = 0; i < len; i++)
        {
            if (tileObjs[colorWay[i]].start) continue;
            if (tileObjs[colorWay[i]].end) continue;
            tileObjs[colorWay[i]].GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    //=====================================================
    // enemy AI : 길찾기 적용 (1. Dijkstra, 2. A*)
    //=====================================================

    public int[] ShortestPath(int x, int y)
    {
#if Astar
        return AStar(x, y);
#elif Dijkstra
        return Dijkstra(x, y);
#endif
    }

    public int min(int a, int b)
    {
        return a < b ? a : b;
    }

    //--------------------------------------------
    // Dijkstra part
    //--------------------------------------------

    private void DijkstraAdj()
    {
        //adj init
        adj = new pair[MAX_V][];

        for (int i = 0; i < MAX_V; i++)
        {
            //adj List init
            adj[i] = new pair[4];
            for (int j = 0; j < 4; j++)
            {
                adj[i][j] = new pair(); //first cost, second here idx
                adj[i][j].first = INF;
                adj[i][j].second = -1;
            }

            //direction
            int curX = (i % (max_X + 1));
            int curY = (i / (max_X + 1));
            int L, R, U, D;
            int L_idx, R_idx, U_idx, D_idx;

            L = curX - 1;
            R = curX + 1;
            U = curY - 1;
            D = curY + 1;

            //x * y : index
            L_idx = L + curY * (max_X + 1);
            R_idx = R + curY * (max_X + 1);
            U_idx = curX + U * (max_X + 1);
            D_idx = curX + D * (max_X + 1);

            //insert index
            //first : cost
            //second : idx
            if (L > -1 && L < max_X + 1)
            {
                adj[i][0].first = tileObjs[L_idx].weight;
                adj[i][0].second = L_idx;
            }

            if (R > -1 && R < max_X + 1)
            {
                adj[i][1].first = tileObjs[R_idx].weight;
                adj[i][1].second = R_idx;
            }

            if (U > -1 && U < max_Y + 1)
            {
                adj[i][2].first = tileObjs[U_idx].weight;
                adj[i][2].second = U_idx;
            }

            if (D > -1 && D < max_Y + 1)
            {
                adj[i][3].first = tileObjs[D_idx].weight;
                adj[i][3].second = D_idx;
            }
        }
    }

    //인접행렬로 구현
    public int[] Dijkstra(int x, int y)
    {
        //start Point
        int src = y * (max_X + 1) + x;

        int[] dist = new int[MAX_V];
        bool[] visited = new bool[MAX_V];

        for (int i = 0; i < MAX_V; i++)
        {
            dist[i] = INF;
            visited[i] = false;
        }

        dist[src] = 0;
        visited[src] = false;

        while (true)
        {
            //아직 방문하지 않은 정점 중 가장 가까운 점을 찾는다.
            int closest = INF, here = -1;
            for (int i = 0; i < MAX_V; i++)
            {
                if (dist[i] < closest && !visited[i])
                {
                    here = i;
                    closest = dist[i];
                }
            }

            if (closest == INF)
                break;
            //가장 가까운 정점을 방문
            visited[here] = true;

            for (int i = 0; i < 4; i++) //인접 정점 탐색
            {
                //실시간으로 벽이 생성되었다면 adj도 바뀌어야 함.
                int there = adj[here][i].second;
                if (there < 0 || there >= MAX_V) continue;
                if (visited[there]) continue;
                int nextDist = dist[here] + adj[here][i].first;
                dist[there] = min(dist[there], nextDist);
            }
        }

        return dist;
    }

    //--------------------------------------------
    // A* part
    //--------------------------------------------

    //맨해튼 거리 
    public int distance1(int x1, int y1, int x2, int y2)
    {
        //|x1 - x2| + |y1 + y2|
        int dx = (x1 - x2) > 0 ? (x1 - x2) : (x1 - x2) * -1;
        int dy = (y1 - y2) > 0 ? (y1 - y2) : (y1 - y2) * -1;
        int dis = dx + dy;

        return dis;
    }

    //유클리드 거리
    public int distance2(int x1, int y1, int x2, int y2)
    {
        //(x1 - x2)^2 + (y1-y2)^2
        float a = x1 - x2;
        float b = y1 - y2;
        float dis = Mathf.Sqrt(a * a + b * b);
        return (int)dis;
    }

    //타일의 가중치 + 이전 노드에서 현 노드까지의 거리
    public int Gcost(int x1, int y1, int x2, int y2)
    {
        int dis = distance1(x1, y1, x2, y2);
        //int dis = distance2(x1, y1, x2, y2);
        return tileObjs[y2 * (max_X + 1) + x2].weight + dis;
    }

    //현 위치에서 End까지의 거리
    public int Hcost(int x1, int y1)
    {
        return distance1(x1, y1, eX, eY);
        //return distance2(x1, y1, eX, eY);
    }

    public List<TileSc> OpenList;
    public List<TileSc> ClosedList;
    public List<TileSc> FindList;
    public TileSc curNode;

    //가중치 + 휴리스틱 거리 
    public int[] AStar(int x, int y)
    {
        OpenList = new List<TileSc>();
        ClosedList = new List<TileSc>();
        FindList = new List<TileSc>();

        TileSc startNode = tileObjs[y * (max_X + 1) + x];
        TileSc endNode = tileObjs[eY * (max_X + 1) + eX];

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            curNode = OpenList[0];
            
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].Fcost <= curNode.Fcost &&
                    OpenList[i].Hcost < curNode.Hcost)
                {
                    curNode = OpenList[i];
                }
            }

            OpenList.Remove(curNode);
            ClosedList.Add(curNode);

            if (curNode == endNode)
            {
                //재탐색
                TileSc TargetCurNode = endNode;
                while (TargetCurNode != startNode)
                {
                    FindList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.parent;
                }
                FindList.Add(startNode);
                FindList.Reverse();

                int[] finalDist = new int[FindList.Count];
                //print FindList
                for (int i = 0; i < FindList.Count; i++)
                {
                    finalDist[i] = (int)FindList[i].y * (max_X + 1) + (int)FindList[i].x;
                }

                return finalDist;
            }

            //direction search (LRUD)
            OpenListAdd((int)curNode.x - 1, (int)curNode.y); // L
            OpenListAdd((int)curNode.x + 1, (int)curNode.y); // R
            OpenListAdd((int)curNode.x, (int)curNode.y - 1); // U
            OpenListAdd((int)curNode.x, (int)curNode.y + 1); // D

#if diagonal //대각선
            //direction search (대각선)
            OpenListAdd((int)curNode.x - 1, (int)curNode.y + 1); // LD
            OpenListAdd((int)curNode.x + 1, (int)curNode.y + 1); // RD
            OpenListAdd((int)curNode.x - 1, (int)curNode.y - 1); // LU
            OpenListAdd((int)curNode.x - 1, (int)curNode.y - 1); // RU
#endif
        }

        Debug.LogWarning("Astar 에서 길을 못 찾았습니다.");
        Time.timeScale = 0;
        ExceptionUI.SetActive(true);

        return null;
    }

    private void OpenListAdd(int nextX, int nextY)
    {
        //상하좌우 범위 벗어나면 탐색안함.
        if (nextX < 0 || nextX >= max_X + 1 || nextY < 0 || nextY >= max_Y + 1)
            return;

        //벽이거나 닫힌리스트에 있으면 리턴
        if (tileObjs[nextY * (max_X + 1) + nextX].weight == 100 ||
            ClosedList.Contains(tileObjs[nextY * (max_X + 1) + nextX]))
            return;

        TileSc nextNode = tileObjs[nextY * (max_X + 1) + nextX];

        //cost = Gcost + weight;
        int moveCost = curNode.Gcost + Gcost((int)curNode.x, (int)curNode.y, nextX, nextY) + curNode.weight + nextNode.weight;
        
        if (moveCost < nextNode.Gcost || !OpenList.Contains(nextNode))
        {
            nextNode.Gcost = moveCost;
            nextNode.Hcost = Hcost((int)nextNode.x, (int)nextNode.y);
            nextNode.parent = curNode;

            OpenList.Add(nextNode);
        }
    }

    public Stack<int> findWay(int[] dist)
    {
        //first idx, second value
        Queue<pair> queue = new Queue<pair>();
        Stack<int> wayList = new Stack<int>();

        int x = max_X;
        int y = max_Y;
        int len = (x + 1) * (y + 1);

        //init visited
        bool[] visited = new bool[len];
        for (int i = 0; i < len; i++)
            visited[i] = false;

        //push destination idx
        int eX = GroundManager.instance.eX;
        int eY = GroundManager.instance.eY;
        int nowIdx = eY * (max_X + 1) + eX;

        queue.Enqueue(new pair(nowIdx, dist[nowIdx]));
        visited[len - 1] = true;

        while (queue.Count != 0)
        {
            //현재 값
            int curIdx = queue.Peek().first;
            int curVal = queue.Peek().second;

            wayList.Push(curIdx);
            queue.Dequeue();

            //L R U D search
            int curX = (curIdx % (x + 1));
            int curY = (curIdx / (x + 1));

            int Lx = (curX - 1);
            int Ly = curY;
            int L = Ly * (x + 1) + Lx;

            int Rx = (curX + 1);
            int Ry = curY;
            int R = Ry * (x + 1) + Rx;

            int Ux = curX;
            int Uy = (curY + 1);
            int U = Uy * (x + 1) + Ux;

            int Dx = curX;
            int Dy = (curY - 1);
            int D = Dy * (x + 1) + Dx;

            //L search
            if (Lx > -1 && Lx < x + 1)
            {
                if (!visited[L] && dist[L] == curVal - 1)
                {
                    queue.Enqueue(new pair(L, dist[L]));
                    visited[L] = true;
                    continue;
                }
            }

            //R search
            if (Rx > -1 && Rx < x + 1)
            {
                if (!visited[R] && dist[R] == curVal - 1)
                {
                    queue.Enqueue(new pair(R, dist[R]));
                    visited[R] = true;
                    continue;
                }
            }

            //U search
            if (Uy > -1 && Uy < y + 1)
            {
                if (!visited[U] && dist[U] == curVal - 1)
                {
                    queue.Enqueue(new pair(U, dist[U]));
                    visited[U] = true;
                    continue;
                }
            }

            //D search
            if (Dy > -1 && Dy < y + 1)
            {
                if (!visited[D] && dist[D] == curVal - 1)
                {
                    queue.Enqueue(new pair(D, dist[D]));
                    visited[D] = true;
                    continue;
                }
            }
        }

        return wayList;
    } //findWay

    //A* 의 경우 Dijkstra와 달리 가중치 계산이 다름.
    //기존 findWay의 경우 이동가능한 타일 가중치를 계산한 값을 처리하고 있기에, 이를 
    //따라서 따로 로직을 구현
    public Stack<int> findWayAstar(int[] dist)
    {
        //로직 구현 시 스택을 반환하는건 같으나, A*의 경우 이미 경로를 구했기 때문에,
        //이 정보를 뒤집어, 그 정보를 스택에 삽입하는 형식으로 가야함.
        
        //Reverse dist
        int distLen = dist.Length;
        for(int i=0;i<distLen/2;i++)
        {
            int tmp = dist[i];
            dist[i] = dist[distLen - 1 - i];
            dist[distLen - 1 - i] = tmp;
        }

        Stack<int> wayList = new Stack<int>();

        for(int i=0;i<distLen;i++)
        {
            wayList.Push(dist[i]);
        }

        return wayList;
    } //findWayAstar

}
