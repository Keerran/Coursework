using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SaveLoad
{
    public static List<Game> savedGames = new List<Game>();
    
    public static void Save()
    {
        savedGames.Add(Game.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        Debug.Log(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, savedGames);
        file.Close();
    }
    public static void Load()
    {
        Debug.Log(Application.persistentDataPath);
        if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
    }

    
}
