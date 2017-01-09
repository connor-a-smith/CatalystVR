using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    //Is this running in the cave?
    public static bool isCave = false;

    public static Controller instance;

    [SerializeField]
    private float cameraEyeOffset = 0.6f;

    [SerializeField]
    public GameObject[] leftEyeCameras;

    [SerializeField]
    public GameObject[] rightEyeCameras;

    [SerializeField]
    public BookmarkController bookmarks;

    [SerializeField]
    public GameObject bookmarkPOIList;

    [SerializeField]
    public Object bookmarkPrefab;

    [SerializeField]
    public InputGuideController inputGuide;

    [SerializeField]
    public GameObject bookmarkPanel;

    [SerializeField]
    public GameObject controllerPanel;

    [SerializeField]
    public GameObject fadePlane;

    [HideInInspector]
    public List<POIScript> POIList;

    public GameObject platformMonitor;
    public GameObject platformModel;

    [SerializeField]
    public bool is3D = false;

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
    public Camera raycastCam;
    public Text monitorText;

    public delegate void ControllerReady();

    public static ControllerReady controllerReady;

    public Object labelPrefab;

    public static MonitorButtonScript[] buttons;

    public static string instructionText = "Welcome to the CAVEkiosk!\n\nUse the Xbox controller to interact with this exhibit, and hold the Right Trigger for a list of detailed controls.";

    public delegate void Toggle3DController();

    public Toggle3DController Toggle3DDelegate;

    // Use this for initialization
    /// <summary>
    /// Used to set up the static variables based on the dragged in variables.
    /// </summary>
    void Awake()
    {

        if (Controller.instance == null)
        {
            Controller.instance = this;

            GameObject topLevelParent = gameObject;

            while (topLevelParent.transform.parent != null)
            {
                topLevelParent = topLevelParent.transform.parent.gameObject;
            }

            POIList = new List<POIScript>();

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

            Controller.instance.monitorText.text = Controller.instructionText;

            ActivateDisplays(new Scene(), LoadSceneMode.Additive);


        }

        //Controller already exists, move the existing to this position and gete rid of this one.
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

    public void FillPOIListOnLoad(Scene scene, LoadSceneMode mode)
    {

        controllerReady();

    }

    public void Start()
    {

        SceneManager.sceneUnloaded += CleanPOI;
        SceneManager.sceneLoaded += FillPOIListOnLoad;
        SceneManager.sceneLoaded += FillPOIList;

        if (controllerReady != null)
        {
            controllerReady();
        }

        ResetCameraPositions();

        monitor.GetComponentInChildren<Text>().text = Controller.instructionText;


        FillPOIList(new Scene(), LoadSceneMode.Single);

    }

    public void FillPOIList(Scene scene, LoadSceneMode mode)
    {

        List<Vector3> panelPositions = new List<Vector3>();

        Rect panelRect = bookmarkPOIList.GetComponent<RectTransform>().rect;

        float panelHeight = panelRect.height;

        float buttonHeight = panelHeight / POIList.Count;

        float buttonPadding = buttonHeight / 4.0f;

        buttonHeight -= buttonPadding;

        float heightPilot = panelRect.yMax - (buttonHeight / 2);

        for (int i = 0; i < POIList.Count; i++)
        {

            panelPositions.Add(new Vector3(0.0f, heightPilot, 0.0f));

            heightPilot -= buttonHeight;
            heightPilot -= buttonPadding;

        }

        for (int i = 0; i < POIList.Count; i++)
        {

            GameObject newPanel = GameObject.Instantiate(bookmarkPrefab, panelPositions[i], Quaternion.identity, bookmarkPOIList.transform) as GameObject;

            RectTransform newTransform = newPanel.GetComponent<RectTransform>();
            newTransform.localPosition = Vector3.zero;
            newTransform.localRotation = Quaternion.identity;
            newTransform.localScale = Vector3.one;

            newTransform.localPosition = panelPositions[i];

            newPanel.name = POIList[i].POIName;

            Text panelText = newPanel.GetComponentInChildren<Text>();

            panelText.text = POIList[i].POIName;

            Image childImage = newTransform.GetComponentInChildren<Image>();
            RectTransform imageTransform = childImage.GetComponent<RectTransform>();

            Vector3 sizeDelta = imageTransform.sizeDelta;
            sizeDelta.y = (buttonHeight * (1.0f / imageTransform.localScale.y));

            Bookmark newBookmark = newPanel.GetComponentInChildren<Bookmark>();
            newBookmark.POI = POIList[i];
            newBookmark.focusComponent = POIList[i].GetComponentInChildren<FocusTransformComponent>();

            bookmarks.bookmarks.Add(newBookmark);

            imageTransform.sizeDelta = sizeDelta;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            ActivateDisplays(new Scene(), LoadSceneMode.Additive);

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Toggle3D();
        }

        if (is3D && Input.GetKey(KeyCode.Minus))
        {
            if (cameraEyeOffset > 0)
            {

                cameraEyeOffset -= 0.01f;
                ResetCameraPositions();
                Debug.LogErrorFormat("Camera Offset: {0}", cameraEyeOffset);
            }

        }

        if (is3D && Input.GetKey(KeyCode.Equals))
        {

            cameraEyeOffset += 0.01f;
            ResetCameraPositions();
            Debug.LogErrorFormat("Camera Offset: {0}", cameraEyeOffset);

        }
    }


    void ActivateDisplays(Scene scenes, LoadSceneMode mode)
    {
        foreach (Display display in Display.displays)
        {
            display.Activate();
            //Debug.LogError("Display number: " + counter + "\n");
            //counter++;
        }
    }

    public void Toggle3D()
    {


        if (is3D)
        {
            Make2D();
        }
        else
        {
            Make3D();
        }
        if (Toggle3DDelegate != null)
        {
            Toggle3DDelegate();
        }
    }

    public void Make2D()
    {
        OffsetCameras(leftEyeCameras, cameraEyeOffset / 2);
        OffsetCameras(rightEyeCameras, -cameraEyeOffset / 2);
        is3D = false;
    }

    public void Make3D()
    {
        OffsetCameras(leftEyeCameras, -cameraEyeOffset / 2);
        OffsetCameras(rightEyeCameras, cameraEyeOffset / 2);
        is3D = true;
    }

    public void OffsetCameras(GameObject[] cameras, float offset)
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

    private void ResetCameraPositions()
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

    public void CleanPOI(Scene scene)
    {

        POIList.Clear();
        bookmarks.ClearBookmarks();

    }

    public IEnumerator SetIdleRotatePlatform() {

        while(true) {

            float rotateAngle = Time.deltaTime * 5.0f;

            playerShip.transform.Rotate(0, rotateAngle, 0, Space.Self);

            yield return null;

        }
    }
}
