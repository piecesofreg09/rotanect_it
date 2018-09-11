using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections;

public class SaveLoadRecords
{
    // Corresponds to the Score class
    public static int[] savedScore = new int[6];

    public static void Save()
    {
        savedScore = Score.savedScores;
        Array.Sort(savedScore);
        Array.Reverse(savedScore);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedScore.gd");
        bf.Serialize(file, SaveLoadRecords.savedScore);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedScore.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedScore.gd", FileMode.Open);
            SaveLoadRecords.savedScore = (int[])bf.Deserialize(file);
            file.Close();
        }
        else
        {
            savedScore = new int[6];
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savedScore.gd");
            bf.Serialize(file, SaveLoadRecords.savedScore);
            file.Close();
            Debug.Log("Create New score data and stored.");
        }
    }
}
