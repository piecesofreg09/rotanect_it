using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class changeScene : MonoBehaviour {
	
	public void changeToScene (string sceneChangeTo) {
        SceneManager.LoadScene(sceneChangeTo);
	}
}
