using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class changeAboutSceneButton : MonoBehaviour
{
    private Button btToChange;
    private Image imToChange;
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
        changeButtonPosition();
    }

    void changeButtonPosition()
    {
        // change button frame
        int iHeight = sHeight / 2;
        int iWidth = sWidth * 2 / 3;
        imToChange = GameObject.Find("AboutImage").GetComponent<Image>();
        rTr = imToChange.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, 0, 0);
        rTr.sizeDelta = new Vector2(iWidth, iHeight);

        // change text size
        int fontSizeL = (int)(iHeight / 13.5f);
        tt = GameObject.Find("AboutText").GetComponent<Text>();
        rTr = tt.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, 0, 0);
        rTr.sizeDelta = new Vector2(iWidth, iHeight);
        tt.fontSize = fontSizeL;

        int butHeight = sHeight / 12;
        // change the close button
        btToChange = GameObject.Find("CloseButton").GetComponent<Button>();
        rTr = btToChange.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, (6 - 2.5f * 6) * butHeight * 1 / 2, 0);
        rTr.sizeDelta = new Vector2(sHeight / 12, sHeight / 12);
        //rTr.sizeDelta = new Vector2(butWidthL, butHeight);
    }
}
