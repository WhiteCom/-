using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
실제 길찾기 알고리즘이 적용되어야 하는 부분
필요사항 
- 맵 정보 (현재 자기위치)
- 목표지점
- 실시간으로 탐색하기 위해 Update 함수에 Find Road 함수 집어넣기 (1. 다익스트라, 2. A*)
(길찾기 외 고려사항)
- 체력, 공격력, 방어력, ...
*/
public class Enemy : MonoBehaviour
{
    //=====================================================
    // 길찾기 용
    //=====================================================
    
    private int[] dist;
    private int distLen;
    private Stack<int> wayList;
    private int wayCnt;

    private Vector2 sV, eV;

    private float waitTime = 0.5f;
    private float tickTime = 0.0f;

    //=====================================================
    // 길찾기 이후 고려사항
    //=====================================================
    public int HP;

    //=====================================================
    // 유니티 라이프 사이클
    //=====================================================

    void Start()
    {

        //(0, 0)이 시작점일 때
        dist = GroundManager.instance.ShortestPath();

        wayList = new Stack<int>();

        sV.x = 0;
        sV.y = 0;

        eV.x = 0;
        sV.y = 0;

        prevWork(dist);
    }
    
    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (tickTime < waitTime)
        {
            tickTime += Time.deltaTime;
        }
        else
        {
            tickTime = 0.0f;
            Moving();
        }

    }

    //find Enemy way
    public void prevWork(int[] dist)
    {
        for (int i = 0; i < distLen; i++)
            Debug.Log(dist[i]);

        //first idx, second value
        Queue<pair> queue = new Queue<pair>();
        wayCnt = 0;

        int x = GroundManager.instance.max_X;
        int y = GroundManager.instance.max_Y;
        int len = (x + 1) * (y + 1);

        //init visited
        bool[] visited = new bool[len];
        for (int i = 0; i < len; i++)
            visited[i] = false;

        //push destination idx
        int eX = GroundManager.instance.eX;
        int eY = GroundManager.instance.eY;
        int max_X = GroundManager.instance.max_X;
        int nowIdx = eY * (max_X+1) + eX;

        //queue.Enqueue(new pair(len-1, dist[len-1]));
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
                if(!visited[U] && dist[U] == curVal - 1)
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
    }

    public void Moving()
    {
        //Test code
        int xNum = GroundManager.instance.max_X;
        int yNum = GroundManager.instance.max_Y;

        int len = (xNum + 1) * (yNum + 1);

        TileSc[] tiles = GroundManager.instance.tileObjs;

        sV.x = transform.position.x;
        sV.y = transform.position.y;

        int idx = 0;

        if (wayList.Count == 0) prevWork(dist); //init wayList
        if(wayList.Count != 0)
        {
            idx = wayList.Peek();
            wayList.Pop();
        }

        eV.x = tiles[idx].transform.position.x;
        eV.y = tiles[idx].transform.position.y;

        Vector2 v = eV - sV;

        transform.Translate(v);
    }
}
