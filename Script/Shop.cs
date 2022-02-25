using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public GameObject Btn;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void AfterClick()
    {
        this.gameObject.SetActive(false);
        Btn.SetActive(true);
    }

    public void BuyWall()
    {
        buildManager.SetBuildObj(buildManager.Wall);
        AfterClick();
    }
}
