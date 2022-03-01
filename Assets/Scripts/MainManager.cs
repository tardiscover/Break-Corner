using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public Brick slidingBrickPrefab;
    public int rowsOfBricks;
    public readonly int maxRowsOfBricks = 6;
    private int bricksPerRow = 6;
    public int bricksLeft = 0;

    public Rigidbody ball;
    public Rigidbody leftPaddle;
    public Rigidbody rightPaddle;

    public Text scoreText;
    public Text bestScoreText;
    public GameObject StartText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public AudioClip gameOverSound;
    private AudioSource mainAudioSource;

    public InputActionAsset primaryActions;
    InputActionMap uiActionMap;
    InputAction restartInputAction;

    private Vector3 ballOffset = new Vector3(0f, 0.15f, 0f);

    //Initialize the Inputs, to be called in Awake().
    private void InitializeInputs()
    {
        uiActionMap = primaryActions.FindActionMap("UI");
        restartInputAction = uiActionMap.FindAction("Restart");
        restartInputAction.performed += HandleRestart;
    }

    //Remove the Inputs when script is destoyed so no orphaned reference (because InputAction not destroyed), 
    //to be called in OnDestroy().
    private void RemoveInputs()
    {
        restartInputAction.performed -= HandleRestart;
    }

    private void Awake()
    {
        InitializeInputs();
    }

    void Start()
    {
        rowsOfBricks = 1;  //First time bricks are initialized, there is 1 row.  This will increase each time they are all eliminated.

        mainAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        ResetScene();
    }

    private void OnEnable()
    {
        restartInputAction.Enable();
    }

    private void OnDisable()
    {
        restartInputAction.Disable();
    }

    private void OnDestroy()
    {
        RemoveInputs();
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

    void DestroyChildren(GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void InitBricks()
    {
 
        GameObject rightBrickWall = GameObject.Find("RightBrickWall");
        GameObject leftBrickWall = GameObject.Find("LeftBrickWall");

        DestroyChildren(rightBrickWall);
        DestroyChildren(leftBrickWall);
        bricksLeft = 0; //Reinitialize before creating more bricks

        // ABSTRACTION
        CreateBrickWall(rowsOfBricks, bricksPerRow, rightBrickWall.transform);
        CreateBrickWall(rowsOfBricks, bricksPerRow, leftBrickWall.transform);
    }

    void ThrowBall()    //!!!!Move to Ball script?
    {
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        ball.transform.SetParent(null);
        ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    void HandleRestart(CallbackContext context)
    {
        if (!m_Started)
        {
            StartText.SetActive(false);

            m_Started = true;
            ThrowBall();
        }
        else if (m_GameOver)
        {
            ResetScene();
        }
        //else ignore
    }

    void ResetPaddles()
    {
        rightPaddle.gameObject.GetComponent<PlayerController>().ResetPosition();
    }

    void ResetBall()
    {
        ball.velocity = Vector3.zero;

        //Add to the right paddle, set on top, and constrain to paddle's direction
        ball.transform.SetParent(rightPaddle.transform);    
        ball.transform.SetPositionAndRotation(rightPaddle.position + ballOffset, ball.rotation);
        ball.gameObject.GetComponent<Ball>().frameWithin = FrameWithin.Right;
        ball.GetComponent<Ball>().ConstrainZTo(0.0f);

        ball.gameObject.SetActive(true);
    }

    void ResetScene()
    {
        //!!!!!
        m_Started = false;
        m_GameOver = false;

        StartText.SetActive(true);
        GameOverText.SetActive(false);
        UpdateBestScoreText();
        InitBricks();
        ResetBall();
        ResetPaddles();
    }

    //private void Update()
    //{

    //}

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
