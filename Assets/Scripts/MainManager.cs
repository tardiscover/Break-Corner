using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public Brick slidingBrickPrefab;
    public int rowsOfBricks;
    public readonly int maxRowsOfBricks = 6;
    private int bricksPerRow = 6;
    public int bricksLeft = 0;

    public Rigidbody ball;

    public Text scoreText;
    public Text bestScoreText;
    public GameObject StartText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public AudioClip gameOverSound;
    private AudioSource mainAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        rowsOfBricks = 1;  //First time bricks are initialized, there is 1 row.  This will increase each time they are all eliminated.

        mainAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        UpdateBestScoreText();
        InitBricks();
    }

    private void CreateBrickWall(int numRows, int numCols, Transform parent)
    {
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        int arrayLength = pointCountArray.Length;
        float rowOffset = 0.3f;
        float colOffset = 0.6f;
        Vector3 position;
        Brick brick;
        int slidingBrickCol;

        for (int row = 0; row < numRows; ++row)
        {
            //For each row, pick a random column to be sliding
            slidingBrickCol = Random.Range(0, arrayLength - 1);

            for (int col = 0; col < numCols; ++col)
            {
                position = parent.transform.TransformPoint((float)col * colOffset, (float)row * rowOffset, 0.0f);
                if (col == slidingBrickCol)
                //if (col == 0)
                {
                    brick = Instantiate(slidingBrickPrefab, position, parent.rotation, parent);
                }
                else
                {
                    brick = Instantiate(brickPrefab, position, parent.rotation, parent);
                }
                bricksLeft++;

                //Assign points based on array, or use last in array if array not long enough
                brick.PointValue = row < arrayLength ? pointCountArray[row] : pointCountArray[arrayLength - 1];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    public void InitBricks()
    {
        bricksLeft = 0;

        // ABSTRACTION
        CreateBrickWall(rowsOfBricks, bricksPerRow, GameObject.Find("RightBrickWall").transform);

        CreateBrickWall(rowsOfBricks, bricksPerRow, GameObject.Find("LeftBrickWall").transform);
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartText.SetActive(false);

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

    // ABSTRACTION
    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        UpdateBestScore();
        mainAudioSource.PlayOneShot(gameOverSound, 1.0f);
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
