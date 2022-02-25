using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life_position : MonoBehaviour
{
    //16:9 ±‚¡ÿ
    void Start()
    {
        float fScaleWidth = ((float)Screen.width / (float)Screen.height) / ((float)16 / (float)9);
        Vector3 vecTextPos = GetComponent<RectTransform>().localPosition;
        vecTextPos.x = vecTextPos.x * fScaleWidth;
        GetComponent<RectTransform>().localPosition = new Vector3(vecTextPos.x, vecTextPos.y, vecTextPos.z);
    }
}
