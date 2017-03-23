using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Is this running in the cave?
    public static bool isCave = false;

    public static GameManager instance;

    public GameObject user;

    [HideInInspector]
    public CatalystPlatform platform;

    [HideInInspector]
    public CAVECameraRig cameraRig;

    [HideInInspector]
    public PhotoController photoController;

    [SerializeField]
    public InputGuideController inputGuide;

    [SerializeField]
    public GameObject controllerPanel;

    [SerializeField]
    public GameObject fadePlane;

    public static PlatformMonitor monitor;
    public static InputManagerScript inputManager;

    public static Object photoPrefab;
    public Object photoPrefabEditor;
    public Camera raycastCam;
    public Text monitorText;

    public Object labelPrefab;

    public static MonitorButtonScript[] buttons;

    public static string instructionText = "Welcome to the CAVEkiosk!\n\nUse the Xbox controller to interact with this exhibit, and hold the Right Trigger for a list of detailed controls.";


    public enum State
    {

        IDLE,
        ACTIVE

    }

    public static State gameState = State.ACTIVE;

    // Use this for initialization
    /// <summary>
    /// Used to set up the static variables based on the dragged in variables.
    /// </summary>
    void Awake()
    {

        HandleSingleton();
        SetupGameManagers();

    }

    public void HandleSingleton()
    {


        if (GameManager.instance == null)
        {
            GameManager.instance = this;

            GameObject topLevelParent = gameObject;

            while (topLevelParent.transform.parent != null)
            {
                topLevelParent = topLevelParent.transform.parent.gameObject;
            }


            DontDestroyOnLoad(topLevelParent);

            photoPrefab = photoPrefabEditor;

            buttons = monitor.GetComponentsInChildren<MonitorButtonScript>();

            for (int i = 0; i < GameManager.buttons.Length; i++)
            {
                GameManager.buttons[i].gameObject.SetActive(false);
            }

            GameManager.instance.monitorText.text = GameManager.instructionText;

        }

        //Controller already exists, move the existing to this position and gete rid of this one.
        else
        {

            GameObject instanceObject = GameManager.instance.gameObject;
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

    public void SetupGameManagers()
    {

        platform = user.GetComponentInChildren<CatalystPlatform>();
        cameraRig = user.GetComponentInChildren<CAVECameraRig>();
        monitor = user.GetComponentInChildren<PlatformMonitor>();
        photoController = GetComponentInChildren<PhotoController>();

    }



    public void Start()
    {


        monitor.GetComponentInChildren<Text>().text = GameManager.instructionText;

    }

    public void Update()
    {

        if (GamepadInput.GetDown(GamepadInput.InputOption.START_BUTTON))
        {
            cameraRig.Toggle3D();
        }

        if (GamepadInput.GetDown(GamepadInput.InputOption.BACK_BUTTON))
        {
            SceneManager.LoadScene("MultiDisplayPlanet");
        }


    }
}
