using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class changeRecodsSceneButton : MonoBehaviour
{

    private Button btToChange;
    private Renderer rend;
    private RectTransform rTr;
    private GameObject title;
    private Text tt;
    private int sHeight;
    private int sWidth;
    private string nameSub;

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
        // change the records button
        int butHeight = sHeight / 12;
        int butWidthL = sHeight / 12;
        int butWidthR = (int)(sHeight / 12 * 3.8f);
        int fontSizeL = butHeight * 2 / 3;
        int butXL = -sHeight * 5 * 9 / 256;
        int butXR = butXL + butWidthL / 2 + butWidthR / 2 + butWidthL / 3;

        for (var i = 1; i < 6; i++)
        {
            nameSub = "IdFrame" + i;
            btToChange = GameObject.Find(nameSub).GetComponent<Button>();
            rTr = btToChange.GetComponent<RectTransform>();
            rTr.localPosition = new Vector3(butXL, (6 - 2.25f * i) * butHeight * 1 / 2, 0);
            rTr.sizeDelta = new Vector2(butWidthL, butHeight);
            tt = btToChange.GetComponentInChildren<Text>();
            tt.fontSize = fontSizeL;
            tt.text = string.Format("{0}",i);

            nameSub = "PFrame" + i;
            btToChange = GameObject.Find(nameSub).GetComponent<Button>();
            rTr = btToChange.GetComponent<RectTransform>();
            rTr.localPosition = new Vector3(butXR, (6 - 2.25f * i) * butHeight * 1 / 2, 0);
            rTr.sizeDelta = new Vector2(butWidthR, butHeight);
            tt = btToChange.GetComponentInChildren<Text>();
            tt.fontSize = fontSizeL;
        }

        // change the close button
        btToChange = GameObject.Find("CloseButton").GetComponent<Button>();
        rTr = btToChange.GetComponent<RectTransform>();
        rTr.localPosition = new Vector3(0, (6 - 2.5f * 6) * butHeight * 1 / 2, 0);
        rTr.sizeDelta = new Vector2(sHeight / 12, sHeight / 12);
        //rTr.sizeDelta = new Vector2(butWidthL, butHeight);

    }

}
