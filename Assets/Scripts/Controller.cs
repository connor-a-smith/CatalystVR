using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

    public static Controller instance;

    [SerializeField] private float cameraEyeOffset = 0.6f;
    [SerializeField] private GameObject[] leftEyeCameras;
    [SerializeField] private GameObject[] rightEyeCameras;

    [SerializeField] private bool is3D = false;

    public static GameObject playerShip;
    public static MonitorScript monitor;
    public static InputManagerScript inputManager;

    public static POIScript selectedPOI;

    public static Material defaultPOIMat;
    public static Material highlightedPOIMat;
    public static Material selectedPOIMat;

    public Material defaultPOIMaterialEditor;
    public Material highlightedPOIMaterialEditor;
    public Material selectedPOIMaterialEditor;

    public static Object photoPrefab;
    public Object photoPrefabEditor;

    public static MonitorButtonScript[] buttons;

    public static string instructionText = "Select a POI or grab the sphere to move. The further the sphere from the grab point, the faster you'll go in the direction you moved it.";

    // Use this for initialization
    /// <summary>
    /// Used to set up the static variables based on the dragged in variables.
    /// </summary>
    void Awake () {

        if (Controller.instance == null)
        {
            Controller.instance = this;

            GameObject topLevelParent = gameObject;

            while (topLevelParent.transform.parent != null)
            {
                topLevelParent = topLevelParent.transform.parent.gameObject;
            }

            DontDestroyOnLoad(topLevelParent);

            playerShip = this.gameObject;
            monitor = GetComponentInChildren<MonitorScript>();
            inputManager = GetComponentInChildren<InputManagerScript>();

            defaultPOIMat = defaultPOIMaterialEditor;
            highlightedPOIMat = highlightedPOIMaterialEditor;
            selectedPOIMat = selectedPOIMaterialEditor;

            photoPrefab = photoPrefabEditor;

            buttons = monitor.GetComponentsInChildren<MonitorButtonScript>();

            for (int i = 0; i < Controller.buttons.Length; i++)
            {
                Controller.buttons[i].gameObject.SetActive(false);
            }

            Controller.monitor.GetComponentInChildren<Text>().text = Controller.instructionText;

            activateDisplays(new Scene(), LoadSceneMode.Additive);

        }
        else
        {
            GameObject instanceObject = Controller.instance.gameObject;
            GameObject currentObject = gameObject;

            while (currentObject.transform.parent != null && instanceObject.transform.parent != null)
            {
                instanceObject.transform.localPosition = currentObject.transform.localPosition;
                instanceObject.transform.localRotation = currentObject.transform.localRotation;
                instanceObject.transform.localScale = currentObject.transform.localScale;

                instanceObject = instanceObject.transform.parent.gameObject;
                currentObject = currentObject.transform.parent.gameObject;

            }

            instanceObject.transform.localPosition = currentObject.transform.localPosition;
            instanceObject.transform.localRotation = currentObject.transform.localRotation;
            instanceObject.transform.localScale = currentObject.transform.localScale;

            Destroy(currentObject);

        }
    }

    public void Start()
    {

        resetCameraPositions();

    }

    // Update is called once per frame
    void Update()
    {
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
        offsetCameras(leftEyeCameras, cameraEyeOffset / 2);
        offsetCameras(rightEyeCameras, -cameraEyeOffset / 2);
        is3D = false;
    }

    public void make3D()
    {
        offsetCameras(leftEyeCameras, -cameraEyeOffset / 2);
        offsetCameras(rightEyeCameras, cameraEyeOffset / 2);
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
