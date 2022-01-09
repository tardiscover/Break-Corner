using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class MenuManager : MonoBehaviour
{
    //!!!!public TextMeshProUGUI UserName;
    public TMP_Text BestScoreText;
    public TMP_InputField UserInputField;

    private void InitializeGameData()
    {
        if (GameManager.gameData == null || GameManager.gameData.recentPlayerName == null)
        {
            SaveData.LoadGameData();
        }

        if (GameManager.gameData != null && BestScoreText != null)
        {
            if (GameManager.gameData.highScorePlayerName != "" && GameManager.gameData.highScore != 0)
            {
                BestScoreText.text = "Best Score : " + GameManager.gameData.highScorePlayerName + " : " + GameManager.gameData.highScore;
            }
            else
            {
                BestScoreText.text = "Best Score: " + GameManager.gameData.highScore;
            }
        }

        if (GameManager.gameData != null && UserInputField != null)
        {
            UserInputField.text = GameManager.gameData.recentPlayerName;
        }
    }

    private void ResetHighScore()
    {
        GameManager.gameData.highScorePlayerName = "";
        GameManager.gameData.highScore = 0;
        InitializeGameData();
    }

    private void LoadMainScene()
    {
        GameManager.gameData.recentPlayerName = UserInputField.text;
        SceneManager.LoadScene("Main");
    }

    public void LoadStartMenuScene()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    //-------------

    private void Start()
    {
        InitializeGameData();
    }

    public void StartButtonClick()
    {
        if (UserInputField.text == "secre#rese#  ")
        {
            //Secret way to reset high score
            ResetHighScore();
        }
        else
        {
            LoadMainScene();
        }
    }

    public void QuitButtonClick()
    {
        Exit();
    }
}
