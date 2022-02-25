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
        //���� �� ���� ��ġ�� �������� �����ϰ��� ��.
        //Ÿ�� �� �ٸ��� �Ǽ��Ϸ��� ���踦 �ٲ�� �ϴ� �κ���.

        SpriteRenderer BuildObj = buildManager.GetBuildObj();

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.GetBuildObj() == null)
            return;

        if (ts.build)
        {
            //Wall �������� �ٲ���.
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
