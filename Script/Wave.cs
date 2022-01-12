using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public Transform startP, endP;

    //=====================================================
    // 오브젝트 풀링
    //=====================================================
    public GameObject enemyPrefab;

    public GameObject[] enemyPool;
    public int enemyCount;

    private int currentIndex = 0;
    private int prevIndex = 0;
    private float nowTime = 0.0f;
    private float waitTime = 2.0f;

    //=====================================================
    // 유니티 라이프 사이클
    //=====================================================
    void Start()
    {
        enemyCreate();
    }

    void Update()
    {
        if(nowTime < waitTime)
        {
            nowTime += Time.deltaTime;
        }
        else
        {
            nowTime = 0.0f;

            enemyAllocate();
        }

        enemyDelete();
    }

    //=====================================================
    // 오브젝트 풀링
    //=====================================================
    public void enemyCreate()
    {
        if (enemyCount == 0)
            enemyCount = 1;

        enemyPool = new GameObject[enemyCount];
        for (int i = 0; i < enemyCount; i++)
        {
            enemyPool[i] = Instantiate(enemyPrefab, startP.position, Quaternion.identity);
            enemyPool[i].SetActive(false);
        }
    }
    
    public void enemyAllocate()
    {
        enemyPool[currentIndex].transform.position = startP.position;
        enemyPool[currentIndex].SetActive(true);

        currentIndex++;
        if (currentIndex >= enemyCount)
        {
            currentIndex = 0;
        }
    }

    public void enemyDelete()
    {
        for(int i=0;i<enemyCount;i++)
        {
            if (enemyPool[i].transform.position.x == endP.transform.position.x && 
                enemyPool[i].transform.position.y == endP.transform.position.y)
            {
                enemyPool[i].SetActive(false);
            }
        }
    }

    
}
