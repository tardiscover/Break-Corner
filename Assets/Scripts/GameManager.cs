using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public static GameData gameData;

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

        if (gameData == null)
        {
            gameData = new GameData();
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
