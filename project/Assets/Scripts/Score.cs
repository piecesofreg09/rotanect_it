using UnityEngine;
using System.Collections;

[System.Serializable]
public class Score
{
    // Corresponds to the SaveLoadRecords Class
    public static int[] savedScores;
    public Score()
    {
        savedScores = new int[6];
    }
}
