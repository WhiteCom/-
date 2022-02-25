using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public static int Lives;
    public int startLives;
    public Text livesText;

    public GameObject GameOverUI;

    private void Start()
    {
        if (startLives < 0)
            startLives = 1;

        Lives = startLives;
    }

    void Update()
    {
        livesText.text = "Life : " + PlayerStat.Lives.ToString();
        if (Lives < 1)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
    }
}
