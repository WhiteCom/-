using System.Collections;
using UnityEngine;

public class TileSc : MonoBehaviour
{
    //
    //Tile Info
    //
    public int weight;                //가중치

    public int type;                //Ground, Air, etc...
    public TileSc parent;           //A* 를 위한 부모인덱스
    public int Fcost, Gcost, Hcost;
    public bool build, start, end;  //생성가능 여부, 시작점, 끝점

    public float x, y;
  
    
    void Awake()
    {
        x = transform.position.x;
        y = transform.position.y;
    }

   
}
