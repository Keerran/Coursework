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
        // Add the current game to the savedGames list.
        savedGames.Add(Game.current);
        // Open the file.
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        // Write all the saved games to the file.
        bf.Serialize(file, savedGames);
        //Close the file.
        file.Close();
    }
    public static void Load()
    {
        // If the file exists...
        if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
            // Open the file.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            // Get all the games from the file, casting them to a list
            // of Game object.
            savedGames = (List<Game>)bf.Deserialize(file);
            // Close the file.
            file.Close();
        }
    }

    
}
