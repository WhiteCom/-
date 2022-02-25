using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;

    private SpriteRenderer rend;
    private Color startColor;
    private TileSc ts;

    BuildManager buildManager;
    
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.color;
        ts = GetComponent<TileSc>();

        buildManager = BuildManager.instance;
    }

    void OnMouseDown()
    {
        //현재 딱 벽만 설치할 로직으로 구현하고자 함.
        //타워 등 다른걸 건설하려면 설계를 바꿔야 하는 부분임.

        SpriteRenderer BuildObj = buildManager.GetBuildObj();

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.GetBuildObj() == null)
            return;

        if (ts.build)
        {
            //Wall 세팅으로 바꿔줌.
            ts.build = false;
            ts.weight = 100;
            rend.sprite = BuildObj.sprite;
        }
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.GetBuildObj() == null)
            return;

        if (!ts.build)
        {
            return;
        }

        else
        {
            //rend.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        //rend.color = startColor;
    }
}
