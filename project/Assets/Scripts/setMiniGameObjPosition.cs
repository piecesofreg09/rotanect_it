using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class setMiniGameObjPosition : MonoBehaviour {

    private Button btToChange;
    private RectTransform rTr;
    private Text tt;
    private int sHeight;
    private int sWidth;

    // Use this for initialization
    void Start () {
        sHeight = Screen.height;
        sWidth = Screen.width;
        // change the shape of the button when scene loaded
        changeButtonPosition();
    }


    void changeButtonPosition()
    {
        int butHeight = sHeight / 12;
        // change the close button
        btToChange = GameObject.Find("Close").GetComponent<Button>();
        rTr = btToChange.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, (6 - 2.5f * 6) * butHeight * 1 / 2, 0);
        rTr.sizeDelta = new Vector2(sHeight / 12, sHeight / 12);
        //rTr.sizeDelta = new Vector2(butWidthL, butHeight);

        // change the score text
        int fontSizeS = (int)(sHeight / 32f);
        int fontSizeL = (int)(sHeight / 15f);
        int fontPos = (int)(sHeight / 18f);

        tt = GameObject.Find("ScoreSmall").GetComponent<Text>();
        rTr = tt.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -fontPos+sHeight/2, 0);
        rTr.sizeDelta = new Vector2(fontSizeL+10, fontSizeL + 10);
        tt.fontSize = fontSizeS;

        tt = GameObject.Find("ScoreBig").GetComponent<Text>();
        rTr = tt.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -2*fontPos + sHeight / 2, 0);
        rTr.sizeDelta = new Vector2(fontSizeL + 10, fontSizeL + 10);
        tt.fontSize = fontSizeL;

        tt = GameObject.Find("TimeSmall").GetComponent<Text>();
        rTr = tt.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -2*fontPos + sHeight / 2, 0);
        rTr.sizeDelta = new Vector2(fontSizeL + 10, fontSizeL + 10);
        tt.fontSize = fontSizeS;

        tt = GameObject.Find("TimeBig").GetComponent<Text>();
        rTr = tt.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -3 * fontPos + sHeight / 2, 0);
        rTr.sizeDelta = new Vector2(fontSizeL + 10, fontSizeL + 10);
        tt.fontSize = fontSizeL;

        tt = GameObject.Find("ScoreAveSmall").GetComponent<Text>();
        rTr = tt.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -3*fontPos + sHeight / 2, 0);
        rTr.sizeDelta = new Vector2(fontSizeL + 10, fontSizeL + 10);
        tt.fontSize = fontSizeS;

        tt = GameObject.Find("ScoreAveBig").GetComponent<Text>();
        rTr = tt.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, -4 * fontPos + sHeight / 2, 0);
        rTr.sizeDelta = new Vector2(fontSizeL + 10, fontSizeL + 10);
        tt.fontSize = fontSizeL;
    }
}
