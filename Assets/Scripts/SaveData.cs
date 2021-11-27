using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveData
{
    const string saveFileName = "saveData.json";

    public static void SaveGameData()
    {
        string path = Application.persistentDataPath + "/" + saveFileName;

        //Debug.Log(path);    //!!!!!!!!!!!!

        if (!File.Exists(path))
        {
            File.Create(path);
        }

        string saveFile = JsonUtility.ToJson(GameManager.gameData);

        StreamWriter writer = new StreamWriter(path);
        writer.Write(saveFile);
        writer.Close();
    }

    //public static void LoadGameData()
    //{
    //    string path = Application.persistentDataPath + "/" + saveFileName;

    //    if (File.Exists(path))
    //    {
    //        MainManager.gameData = JsonUtility.FromJson(path, typeof(GameData));
    //    }
    //}
}
