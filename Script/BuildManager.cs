using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public SpriteRenderer Wall;
    private SpriteRenderer BuildObj;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("2�� �̻��� ����Ŵ����� ����");
        }
        instance = this;
    }

    void Update()
    {
        //�Ǽ��ϴ� �߿� ��Ŭ�� �� ���
        if (Input.GetMouseButtonDown(1) && BuildObj != null)
        {
            SetBuildObj(null);
        }
    }

    public SpriteRenderer GetBuildObj()
    {
        return BuildObj;
    }

    public void SetBuildObj(SpriteRenderer sprite)
    {
        BuildObj = sprite;
    }
}
