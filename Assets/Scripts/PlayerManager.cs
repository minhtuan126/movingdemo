using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public static bool isGameStarted;

    public GameObject gameOverPanel;
    public GameObject startingGame;

    public Text coinsText;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        coinsText.text = "COIN : " + PlayerController.numberOfCoins;

        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingGame);
        }
    }
}
