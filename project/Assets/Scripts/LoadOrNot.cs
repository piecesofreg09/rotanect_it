using UnityEngine;
using System.Collections;

[System.Serializable]
public class LoadOrNot
{
    // Corresponds to the LoadOrNotSave Class, when loadOr == true, load game
    public static bool loadOr;
    public LoadOrNot()
    {
        loadOr = new bool();
    }

}
