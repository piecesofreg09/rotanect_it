using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class changeOpenSceneObjSize : MonoBehaviour
{
    private Button btToChange;
    private Renderer rend;
    private RectTransform rTr;
    private GameObject title;
    private Text tt;
    private int sHeight;
    private int sWidth;

    // Use this for initialization
    void Start()
    {

        sHeight = Screen.height;
        sWidth = Screen.width;
        // change the shape of the button when scene loaded
        ChangeButtonPosition();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeButtonPosition()
    {
        // sizes of the button
        int butHeight = sHeight / 9;
        int butWidth = sWidth / 7 * 5;
        int fontSizeL = (int)((float)butHeight * 1.8f / 3);
        Debug.Log("butHeight: " + butHeight + " butWidth: " + butWidth);
        // size of the hints
        int fontSizeS = (int)(sHeight / 48f);


        btToChange = GameObject.Find("Load").GetComponent<Button>();
        rTr = btToChange.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, butHeight / 2, 0);
        rTr.sizeDelta = new Vector2(butWidth, butHeight);
        tt = btToChange.GetComponentInChildren<Text>();
        tt.fontSize = fontSizeL;

        btToChange = GameObject.Find("New").GetComponent<Button>();
        rTr = btToChange.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -butHeight / 2, 0);
        rTr.sizeDelta = new Vector2(butWidth, butHeight);
        tt = btToChange.GetComponentInChildren<Text>();
        tt.fontSize = fontSizeL;
        
        btToChange = GameObject.Find("Records").GetComponent<Button>();
        rTr = btToChange.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -butHeight * 3 / 2, 0);
        rTr.sizeDelta = new Vector2(butWidth, butHeight);
        tt = btToChange.GetComponentInChildren<Text>();
        tt.fontSize = fontSizeL;

        btToChange = GameObject.Find("About").GetComponent<Button>();
        rTr = btToChange.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -butHeight * 5 / 2, 0);
        rTr.sizeDelta = new Vector2(butWidth, butHeight);
        tt = btToChange.GetComponentInChildren<Text>();
        tt.fontSize = fontSizeL;

        // change the position of the hints
        tt = GameObject.Find("Hints").GetComponent<Text>();
        rTr = tt.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -butHeight * 7 / 2, -10);
        rTr.sizeDelta = new Vector2(butWidth, butHeight);
        tt.fontSize = fontSizeS;

    }

}
