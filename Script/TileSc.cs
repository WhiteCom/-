using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSc : MonoBehaviour
{
    public int weight;                //가중치

    public int type;                //Ground, Air, etc...
    public bool build, start, end;  //생성가능 여부, 시작점, 끝점

    public float x, y;

    void Awake()
    {
        x = transform.position.x;
        y = transform.position.y;
    }
}
