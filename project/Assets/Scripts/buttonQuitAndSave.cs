using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class buttonQuitAndSave : MonoBehaviour {

    Button myButton;

    void Awake()
    {
        myButton = GetComponent<Button>(); // <-- you get access to the button component here

        myButton.onClick.AddListener(saveGameData);  // <-- you assign a method to the button OnClick event here
        //myButton.onClick.AddListener(onClickChangeScene); // <-- you can assign multiple methods
    }

    void saveGameData()
    {
        SaveLoad.Save();
    }

    void onClickChangeScene()
    {
        SceneManager.LoadScene("openScene");
    }

}
