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
    public TMP_InputField UserInputField;

    //private void Start()
    //{
    //    UserInputField.interactable = true; //!!!!
    //}

    public void StartNew()
    {
        GameManager.gameData.playerName = UserInputField.text;
        SceneManager.LoadScene("main");
    }

    public void StartMenu()
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
}
