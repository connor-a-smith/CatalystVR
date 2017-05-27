using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class POIManager : MonoBehaviour {

    public static List<POI> POIList;

    public static List<SerializableCatalystSite> siteList;

    [SerializeField] private Object poiPrefab;

    [HideInInspector] public BookmarkController bookmarks;

    public static POI selectedPOI;

    public static Material defaultPOIMat;
    public static Material highlightedPOIMat;
    public static Material selectedPOIMat;

    public Material defaultPOIMaterialEditor;
    public Material highlightedPOIMaterialEditor;
    public Material selectedPOIMaterialEditor;

    private GameManager gameManager;

    private static POIManager instance;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
            
        }
        else
        {

            return;

        }

        defaultPOIMat = defaultPOIMaterialEditor;
        highlightedPOIMat = highlightedPOIMaterialEditor;
        selectedPOIMat = selectedPOIMaterialEditor;

        gameManager = GetComponentInParent<GameManager>();
        bookmarks = gameManager.user.GetComponentInChildren<BookmarkController>();

    }

    public void Start()
    {

        UpdateBookmarks(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        SceneManager.sceneUnloaded += ClearPOIList;
        SceneManager.sceneLoaded += UpdateBookmarks;


    }
    public static void AddPOI(POI POI)
    {

        if (POIList == null)
        {
            POIList = new List<POI>();
        }

        POIList.Add(POI);

    }

    public void UpdateBookmarks(Scene scene, LoadSceneMode mode)
    {

        Debug.LogWarning("Updating Bookmarks");



    }

    public void ClearPOIList(Scene scene)
    {

        POIList.Clear();

        bookmarks.ClearBookmarks();

    }

    private void Update()
    {

        CheckIfPOISelected();

        if (selectedPOI != null && GamepadInput.GetDown(GamepadInput.InputOption.B_BUTTON))
        {

            selectedPOI.Deactivate();

        }
    }
   
    public POI CreateNewPOI(CatalystSite site)
    {

        Vector3 pos = CatalystEarth.Get3DPositionFromLatLon(site.siteData.latitude, site.siteData.longitude);

        Debug.LogFormat("Getting 3D Position of {0}, {1}: ({2}, {3}, {4})", site.siteData.latitude, site.siteData.longitude, pos.x, pos.y, pos.z);

        GameObject newObj = GameObject.Instantiate(poiPrefab, pos, Quaternion.identity) as GameObject;
        POI newPOI = newObj.GetComponent<POI>();
        newPOI.POIName = site.siteName;

        newObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up) * Quaternion.LookRotation(CatalystEarth.earthTransform.position);
        newObj.transform.LookAt(CatalystEarth.earthTransform.position);

        if (POIList == null)
        {
            POIList = new List<POI>();
        }

        newPOI.associatedSite = site;

        POIList.Add(newPOI);

        return newPOI;
    }

    public void CheckIfPOISelected()
    {

        if (selectedPOI == null && GamepadInput.GetDown(GamepadInput.InputOption.A_BUTTON))
        {

            RaycastHit cameraRaycast = CameraViewpoint.GetRaycast();

            if (cameraRaycast.collider != null)
            {

                POI hitPOI = cameraRaycast.collider.GetComponent<POI>();

                if (hitPOI != null)
                {

                    hitPOI.Toggle(gameManager);
                   
                }
            }
        }
    }
}
