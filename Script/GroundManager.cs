using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager instance;

    public int max_X, max_Y;
    public TileSc[] tileObjs;
    public int sX, sY; //start point
    public int eX, eY; //end point

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
        }
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

    private void init()
    {
        tileObjs = FindObjectsOfType<TileSc>();
        Sort();

        int len = tileObjs.Length;
        max_X = (int)tileObjs[len - 1].x;
        max_Y = (int)tileObjs[len - 1].y;

        //////////////////////////////////////////////////////

        MAX_V = (max_X + 1) * (max_Y + 1);

        //adj init
        adj = new pair[MAX_V][];

        for (int i=0;i<MAX_V;i++)
        {
            //TileSc init
            int tx = (int)tileObjs[i].transform.position.x;
            int ty = (int)tileObjs[i].transform.position.y;

            tileObjs[i].curIdx = ty * max_X + ty;
            tileObjs[i].nextIdx = -1;                       //tmp idx
            tileObjs[i].visit = false;

            //find startP
            if(tileObjs[i].start)
            {
                sX = (int)tileObjs[i].transform.position.x;
                sY = (int)tileObjs[i].transform.position.y;
            }
            if(tileObjs[i].end)
            {
                eX = (int)tileObjs[i].transform.position.x;
                eY = (int)tileObjs[i].transform.position.y;
            }

            //adj List init
            adj[i] = new pair[4];
            for(int j=0;j<4;j++)
            {
                adj[i][j] = new pair(); //first cost, second here
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
            L_idx = L       + curY          * (max_X+1); 
            R_idx = R       + curY          * (max_X+1);
            U_idx = curX    + U             * (max_X+1);
            D_idx = curX    + D             * (max_X+1);

            //insert index
            //first : cost
            //second : idx
            if(L > -1 && L < max_X+1)
            {
                adj[i][0].first = tileObjs[L_idx].weight;
                adj[i][0].second = L_idx;
            }

            if(R > -1 && R < max_X+1)
            {
                adj[i][1].first = tileObjs[R_idx].weight;
                adj[i][1].second = R_idx;
            }

            if(U > -1 && U < max_Y+1)
            {
                adj[i][2].first = tileObjs[U_idx].weight;
                adj[i][2].second = U_idx;
            }

            if(D > -1 && D < max_Y+1)
            {
                adj[i][3].first = tileObjs[D_idx].weight;
                adj[i][3].second = D_idx;
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
    // enemy AI : 길찾기 적용 (1. Dijkstra)
    //=====================================================

    public float distance1(Vector2 v1, Vector2 v2)
    {
        //맨해튼 거리 
        //|x1 - x2| + |y1 + y2|
        float dis = (v1.x - v2.x) + (v1.y - v2.y);

        return dis > 0 ? dis : (dis * -1);
    }

    public float distance2(Vector2 v1, Vector2 v2)
    {
        //유클리드 거리
        //(x1 - x2)^2 + (y1-y2)^2
        float a = v1.x - v2.x;
        float b = v1.y - v2.y;
        float dis = Mathf.Sqrt(a * a + b * b);
        return dis;
    }

    private int INF = 9999;
    private int MAX_V;

    private pair[][] adj;

    public int min(int a, int b)
    {
        return a < b ? a : b;
    }

    public int[] ShortestPath()
    {
        int src = sY * (max_X+1) + sX;

        int[] dist = new int[MAX_V];
        bool[] visited = new bool[MAX_V];
        
        for (int i = 0; i < MAX_V; i++)
        {
            dist[i] = INF;
            visited[i] = false;
        }

        dist[src] = 0;
        visited[src] = false;

        while(true)
        {
            //아직 방문하지 않은 정점 중 가장 가까운 점을 찾는다.
            int closest = INF, here = -1;
            for(int i=0;i<MAX_V;i++)
            {
                if(dist[i] < closest && !visited[i])
                {
                    here = i;
                    closest = dist[i];
                }
            }
            
            if (closest == INF)
                break;
            //가장 가까운 정점을 방문
            visited[here] = true;

            for (int i=0;i<4;i++) //인접 정점 탐색
            {
                int there = adj[here][i].second;
                if (there < 0 || there >= MAX_V) continue;
                if (visited[there]) continue;
                int nextDist = dist[here] + adj[here][i].first;
                dist[there] = min(dist[there], nextDist);
            }
        }
        
        return dist;
    }
}
