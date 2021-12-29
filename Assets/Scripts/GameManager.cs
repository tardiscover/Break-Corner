using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public static GameData gameData = new GameData();

    //Make this a singleton that can be accessed from anywhere
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        Instance = this;

        //!!!SceneManager.sceneLoaded += OnSceneLoaded;
    }
}
