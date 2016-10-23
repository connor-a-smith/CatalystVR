using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class DisplayScript : MonoBehaviour {


    [SerializeField] private float cameraEyeOffset = 0.6f;
    [SerializeField] private GameObject[] leftEyeCameras;
    [SerializeField] private GameObject[] rightEyeCameras;

    [SerializeField] private bool is3D = false;

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

        resetCameraPositions();
       // activateDisplays(new Scene(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
	    if (Input.GetKey(KeyCode.B))
        {
            activateDisplays(new Scene(), LoadSceneMode.Additive);

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            toggle3D();
        }

        if (is3D && Input.GetKey(KeyCode.Minus))
        {
            if (cameraEyeOffset > 0)
            {

                cameraEyeOffset -= 0.01f;
                resetCameraPositions();
                Debug.LogErrorFormat("Camera Offset: {0}", cameraEyeOffset);
            }

        }

        if (is3D && Input.GetKey(KeyCode.Equals))
        {

            cameraEyeOffset += 0.01f;
            resetCameraPositions();
            Debug.LogErrorFormat("Camera Offset: {0}", cameraEyeOffset);

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

    public void toggle3D()
    {
        if (is3D)
        {
            make2D();
            return;
        }

        make3D();
    }

    public void make2D()
    {
        offsetCameras(leftEyeCameras, cameraEyeOffset/2);
        offsetCameras(rightEyeCameras, -cameraEyeOffset/2);
        is3D = false;
    }

    public void make3D()
    {
        offsetCameras(leftEyeCameras, -cameraEyeOffset/2);
        offsetCameras(rightEyeCameras, cameraEyeOffset/2);
        is3D = true;
    }

    public void offsetCameras(GameObject[] cameras, float offset)
    {
        foreach (GameObject camera in cameras)
        {
            Vector3 newPosition = camera.transform.localPosition;
            newPosition.x += offset;
            camera.transform.localPosition = newPosition;
        }
    }

    public void moveCameras(GameObject[] cameras, float xVal)
    {
        foreach (GameObject camera in cameras)
        {
            Vector3 newPosition = camera.transform.localPosition;
            newPosition.x = xVal;
            camera.transform.localPosition = newPosition;
        }
    }

    private void resetCameraPositions()
    {
        if (!is3D)
        {
            moveCameras(leftEyeCameras, 0.0f);
            moveCameras(rightEyeCameras, 0.0f);
        }
        else
        {
            moveCameras(leftEyeCameras, -cameraEyeOffset / 2);
            moveCameras(rightEyeCameras, cameraEyeOffset / 2);
        }
    }
}
