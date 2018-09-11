using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class updateRecords : MonoBehaviour {

	// Use this for initialization
	void Start () {

        SaveLoadRecords.Load();
        int[] ttScore = SaveLoadRecords.savedScore;

        for (var i = 0; i < 5; i++)
        {
            string name = "PFrame" + (i + 1);
            Text txtTemp = GameObject.Find(name).GetComponent<Button>().GetComponentInChildren<Text>();
            txtTemp.text = string.Format("{0} p/m",(int)ttScore[i]);
        }
	
	}
	
	// Update is called once per frame
	void Update () {
        SaveLoadRecords.Load();
        int[] ttScore = SaveLoadRecords.savedScore;

        for (var i = 0; i < 5; i++)
        {
            string name = "PFrame" + (i + 1);
            Text txtTemp = GameObject.Find(name).GetComponent<Button>().GetComponentInChildren<Text>();
            txtTemp.text = string.Format("{0} p/m", (int)ttScore[i]);
        }
    }
}
