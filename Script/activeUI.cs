using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeUI : MonoBehaviour
{
    public GameObject Shop;

    public void Click()
    {
        Shop.SetActive(true);
        this.gameObject.SetActive(false);
    }

#if UNITY_WEBPLAYER
    public static string webQuitURL = "http://google.com";
#endif

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webQuitURL);
#else
        Application.Quit();
#endif
    }
}
