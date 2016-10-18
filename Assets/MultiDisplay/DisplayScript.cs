using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class DisplayScript : MonoBehaviour {

	//private Camera[] cameras; //array of all the cameras

	// Use this for initialization
	void Awake() {
        //SceneManager.sceneLoaded+=activateDisplays;
        //DontDestroyOnLoad(gameObject);
        /*
		//searches all the children of this object for Cameras 
		cameras = GetComponentsInChildren<Camera> ();

		foreach (Camera camera in cameras) {
			CameraRig rig = camera.GetComponent<CameraRig> ();
			if (rig != null) {
				rig.SetupCamera (camera, camera.targetDisplay - 1);
			} else {
				Debug.LogError ("Error: Can't find CameraRig");
			}
		}
        */
        //Debug.LogError("displays connected: " + Display.displays.Length);
        //      //loops through each display and activates it
        //      int counter = 0;
        //      Camera[] cameras = GetComponentsInChildren<Camera>();
        //      for (int i = 0; i < cameras.Length; i++)
        //      {
        //          cameras[i].enabled = false;
        //          cameras[i].enabled = true;
        //      }			

/*        foreach (Display display in Display.displays)
        {
            display.Activate();
            //Debug.LogError("Display number: " + counter + "\n");
            //counter++;
        }*/
    }
	
    void Start()

    {
       // activateDisplays(new Scene(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
	    if (Input.GetKey(KeyCode.B))
        {
            activateDisplays(new Scene(), LoadSceneMode.Additive);

        }
    }

    void activateDisplays(Scene scenes, LoadSceneMode mode)
    {
        foreach (Display display in Display.displays)
        {
            display.Activate();
            //Debug.LogError("Display number: " + counter + "\n");
            //counter++;
        }
    }
}
