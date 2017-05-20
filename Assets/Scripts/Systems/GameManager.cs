using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public const string dataDirectory = "./CAVEkiosk_SiteData";
    public const string cacheDirectory = dataDirectory + "/CachedData";
    public const string dataJsonFile = dataDirectory + "/siteData.json";

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

    [HideInInspector]
    public InputGuideController inputGuide;

    [SerializeField]
    public GameObject fadePlane;

    public static PlatformMonitor monitor;

    public static Object photoPrefab;
    public Object photoPrefabEditor;

    public Object labelPrefab;

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

        }
        else
        {
            GameObject.DestroyImmediate(this.gameObject);
            return;
        }
    }

    public void SetupGameManagers()
    {

        if (this != null && user != null) {

            platform = user.GetComponentInChildren<CatalystPlatform>();
            cameraRig = user.GetComponentInChildren<CAVECameraRig>();
            monitor = user.GetComponentInChildren<PlatformMonitor>();
            photoController = GetComponentInChildren<PhotoController>();
            inputGuide = user.GetComponentInChildren<InputGuideController>();
            platform.gameManager = this;
        }
    }

    public static void GoHome()
    {
        SiteManager.activeSite.DeactivateActiveElement();
        SiteManager.activeSite = null;
        SceneManager.LoadScene(0);
        PlatformMonitor.ResetMonitorText();
    }

    public void Update()
    {

        if (GamepadInput.GetDown(GamepadInput.InputOption.START_BUTTON))
        {
            cameraRig.Toggle3D();
        }

        if (GamepadInput.GetDown(GamepadInput.InputOption.BACK_BUTTON))
        {
            // Loads the first scene. Assumed to be the home scene.
            GoHome();
        }
    }
}
