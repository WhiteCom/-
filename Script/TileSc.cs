using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSc : MonoBehaviour
{
    //for move
    public int curIdx;              //Curr node
    public int nextIdx;             //Next node

    public int weight;              //for pathfinding
    public bool visit;              //Check visit

    public int type;                //Ground, Air, etc...
    public bool build, start, end;  //Point

    public float x, y;

    void Awake()
    {
        x = transform.position.x;
        y = transform.position.y;
    }
}
