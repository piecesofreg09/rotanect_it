using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class checkSavedNot : MonoBehaviour {

    private Button loadButton;

    // Use this for initialization
    void Start() {
        LoadOrNotSave.Load();
        LoadOrNot.loadOr = LoadOrNotSave.loadOri;
        loadButton = GameObject.Find("Load").GetComponent<Button>();
        if (!LoadOrNot.loadOr)
        {
            loadButton.interactable = false;
        }

    }
	
	// Update is called once per frame
	void Update () {
	}

    
}
