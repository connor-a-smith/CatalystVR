using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SequentialSceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

    //Load the next scene or go back to 0 if at end.
    if (Input.GetKeyDown("n")) {
      SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % (SceneManager.sceneCount + 1));
    }

    if (Input.GetKeyDown("r")) {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
  }
}
