using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public int lineCount;
    public readonly int maxLineCount = 6;
    public Rigidbody ball;

    public Text scoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    
    public int bricksLeft;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        lineCount = 1;  //!!!
        UpdateBestScoreText();
        InitBricks();
    }

    public void InitBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        Quaternion rotatedBrickQuaternion = Quaternion.Euler(0, 90, 0);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };

        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                //Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                Vector3 position = new Vector3(1.0f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                bricksLeft++;
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        for (int i = 0; i < lineCount; ++i)
        {
            for (int z = 0; z < perLine; ++z)
            {
                //Vector3 position = new Vector3(-2.5f, 2.5f + i * 0.3f, -1.0f + step * -z);
                Vector3 position = new Vector3(0.0f, 2.5f + i * 0.3f, -1.0f + step * -z);
                var brick = Instantiate(brickPrefab, position, rotatedBrickQuaternion);
                bricksLeft++;
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        scoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        UpdateBestScore();
        SaveData.SaveGameData();
    }

    void UpdateBestScoreText()
    {
        if (GameManager.gameData.highScore == 0)
        {
            bestScoreText.text = "Best Score : " + GameManager.gameData.highScore;
        }
        else
        {
            bestScoreText.text = "Best Score : " + GameManager.gameData.highScorePlayerName + " : " + GameManager.gameData.highScore;
        }
    }

    void UpdateBestScore()
    {
        if (m_Points > GameManager.gameData.highScore)
        {
            GameManager.gameData.highScorePlayerName = GameManager.gameData.recentPlayerName;
            GameManager.gameData.highScore = m_Points;
            UpdateBestScoreText();
        }
    }
}
