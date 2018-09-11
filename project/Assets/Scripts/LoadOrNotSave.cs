using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections;

public class LoadOrNotSave : MonoBehaviour {

    public static bool loadOri;
    // Corresponds to LoadOrNot Class
    public static void Save()
    {
        loadOri = LoadOrNot.loadOr;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/loadFlag.gd");
        bf.Serialize(file, LoadOrNotSave.loadOri);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/loadFlag.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/loadFlag.gd", FileMode.Open);
            LoadOrNotSave.loadOri = (bool)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            LoadOrNotSave.loadOri = false;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/loadFlag.gd");
            bf.Serialize(file, LoadOrNotSave.loadOri);
            file.Close();
            Debug.Log("Create New flag data and stored.");
        }
    }
}
