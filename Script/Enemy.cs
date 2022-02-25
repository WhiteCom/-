//-------------------------------------------------------------------
// 전처리기로 Astar 로직으로 할 지 Dijkstra 로직으로 할 지 결정
//-------------------------------------------------------------------

//#define Astar 
#define Dijkstra

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //=====================================================
    // 길찾기 용
    //=====================================================

    private Animator animator;

    private int[] dist;
    private int distLen;
    private int distIdx;
    private Stack<int> wayList; //for Dijkstra

    private bool colorChk;

    private Vector2 sV, eV;

    private float waitTime = 0.5f;
    private float tickTime = 0.0f;

    //
    //for tracking
    //
    public GameObject popChgWay;
    private int[] colorWay; //색깔을 지정할 경로 idx
    private Color changeColor;

    //=====================================================
    // 길찾기 이후 고려사항
    //=====================================================
    public int HP;

    //=====================================================
    // 유니티 라이프 사이클
    //=====================================================

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        GroundManager.instance.init();

        sV.x = transform.position.x;
        sV.y = transform.position.y;

        eV.x = GroundManager.instance.eX;
        eV.y = GroundManager.instance.eY;

        popChgWay = transform.GetChild(0).gameObject;

        //시작점일 때
        dist = GroundManager.instance.ShortestPath((int)sV.x, (int)sV.y);
        distLen = (GroundManager.instance.max_X + 1) * (GroundManager.instance.max_Y + 1);
        distIdx = 0;

        wayList = new Stack<int>();
#if Dijkstra
        wayList = GroundManager.instance.findWay(dist);

        colorWay = wayList.ToArray();
        GroundManager.instance.ResetColor();
        GroundManager.instance.ChangeColor(new Color(1, 0, 1, 1), colorWay);
#elif Astar
        wayList = GroundManager.instance.findWayAstar(dist);

        colorWay = wayList.ToArray();
        GroundManager.instance.ResetColor();
        GroundManager.instance.ChangeColor(new Color(1, 0, 1, 1), colorWay);
#endif

        changeColor = new Color(1, 0, 1, 1);
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
            if (popChgWay.activeSelf == true)
            {
                popChgWay.SetActive(false);
            }
            Moving();
        }

    }

    //벽이 세워졌을 때 실시간 길찾기를 위함.
    public int nextIdx(TileSc[] tiles)
    {
        if (tiles == null)
            Debug.Log("Tiles is Null");
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        //캐릭터가 시작점에서 새롭게 시작되는 경우
        if (wayList.Count == 0)
        {
            dist = GroundManager.instance.ShortestPath((int)sV.x, (int)sV.y);
#if Dijkstra
            wayList = GroundManager.instance.findWay(dist); //init wayList
#elif Astar
            wayList = GroundManager.instance.findWayAstar(dist);
#endif 
            colorWay = wayList.ToArray();

            /////////////////////////////////////////////////////////////////////////////////////
            GroundManager.instance.ResetColor();
            GroundManager.instance.ChangeColor(changeColor, colorWay);
            /////////////////////////////////////////////////////////////////////////////////////
        }

        int idx = -1;
        if (wayList.Count != 0)
        {
            idx = wayList.Peek();
            if (tiles[idx].weight == 100) //중간에 벽이 세워진 경우
            {
                GroundManager.instance.init(); //인접행렬 재구성

                //현 좌표를 기준으로 새롭게 길찾기 시작
                dist = GroundManager.instance.ShortestPath(x, y);
#if Dijkstra
                wayList = GroundManager.instance.findWay(dist);
#elif Astar
                wayList = GroundManager.instance.findWayAstar(dist);
#endif
                colorWay = wayList.ToArray();

                ///////////////////////////////////////////////////////////////////////////////////////////////
                GroundManager.instance.ResetColor();
                GroundManager.instance.ChangeColor(changeColor, colorWay);
                //////////////////////////////////////////////////////////////////////////////////////////////////

                popChgWay.SetActive(true);
            }

            idx = wayList.Peek();
            wayList.Pop();
        }

        //재구성을 했음에도 더이상 갈 곳이 없는 경우
        if (idx == -1 || tiles[idx].weight >= 100)
        {
            //Debug.Log("ExceptionUI!");
            Time.timeScale = 0;
            GroundManager.instance.ExceptionUI.SetActive(true);
        }

        return idx;
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

        int idx = nextIdx(tiles);
        Debug.Log("Enemy가 다음에 이동할 위치 : " + idx);

        eV.x = tiles[idx].transform.position.x;
        eV.y = tiles[idx].transform.position.y;

        Vector2 v = eV - sV;

        int LR = (int)sV.x - (int)eV.x;
        int UD = (int)sV.y - (int)eV.y;
        int dir = 0; //default L

        if (LR < 0) //right
        {
            dir = 1;
            animator.Play("marisa_right");
        }
        else if (LR > 0) //left
        {
            dir = 0;
            animator.Play("marisa_left");
        }
        
        //mirrored up & down in map
        if (UD > 0) //down
        {
            dir = 2;
            animator.Play("marisa_down");
        }
            
        else if (UD < 0) //up
        {
            dir = 3;
            animator.Play("marisa_up");
        }

        transform.Translate(v);
    }
}
