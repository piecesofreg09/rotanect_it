using UnityEngine;
using System.Collections;

public class setLoadFlagFalse : MonoBehaviour {

    // this class is set in the manager delegate, to change the state of load flag
    public void setLFFalse()
    {
        LoadOrNot.loadOr = false;
        Debug.Log("new game icon clicked, load flag set to false");
        LoadOrNotSave.Save();
    }
}
