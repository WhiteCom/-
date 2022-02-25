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
            Debug.LogError("2개 이상의 빌드매니저가 있음");
        }
        instance = this;
    }

    void Update()
    {
        //건설하는 중에 우클릭 시 취소
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
