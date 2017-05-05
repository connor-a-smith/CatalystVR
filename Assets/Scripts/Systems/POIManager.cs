using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class POIManager : MonoBehaviour {

    public static List<POI> POIList;

    public static List<CatalystSite> siteList;

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

        LoadSites();

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



        if (POIList == null)
        {
            LoadSites();
        }

        bookmarks.UpdateBookmarks(POIList);


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

    public void LoadSites()
    {

        if (!File.Exists(GameManager.dataJsonFile))
        {

            SerializableCatalystSites sampleSites = new SerializableCatalystSites();

            sampleSites.sites = new CatalystSite[1];

            CatalystSite newSite = new CatalystSite();

            // UC San Diego Lat/Lon as sample
            newSite.latitude = 32.8801f;
            newSite.longitude = 117.2340f;
            newSite.name = "UC San Diego";
            newSite.description = "This site was generated as a sample of what the JSON file should look like, roughly";

            sampleSites.sites[0] = newSite;

            string jsonText = JsonUtility.ToJson(sampleSites);

            File.WriteAllText(GameManager.dataJsonFile, jsonText);

            return;

        }

        string jsonString = File.ReadAllText(GameManager.dataJsonFile);

        SerializableCatalystSites siteData = JsonUtility.FromJson<SerializableCatalystSites>(jsonString);

        if (siteData.sites != null && siteData.sites.Length > 0)
        {
            siteList = new List<CatalystSite>(siteData.sites);
        }
        else
        {
            Debug.LogErrorFormat("Error: No sites loaded. Please check the following file: {0}", GameManager.dataJsonFile);
            return;
        }

        Debug.LogFormat("Loaded {0} sites", siteList.Count);

        POIList = new List<POI>();

        foreach (CatalystSite site in siteList)
        {

            CreateNewPOI(site);

        }
    }

    public POI CreateNewPOI(CatalystSite site)
    {
        Vector3 pos = CatalystEarth.Get3DPositionFromLatLon(site.latitude, site.longitude);

        Debug.LogFormat("Getting 3D Position of {0}, {1}: ({2}, {3}, {4})", site.latitude, site.longitude, pos.x, pos.y, pos.z);

        GameObject newObj = GameObject.Instantiate(poiPrefab, pos, Quaternion.identity) as GameObject;
        POI newPOI = newObj.GetComponent<POI>();
        newPOI.POIName = site.name;

        newObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up) * Quaternion.LookRotation(CatalystEarth.earthTransform.position);
        newObj.transform.LookAt(CatalystEarth.earthTransform.position);

        if (POIList == null)
        {
            POIList = new List<POI>();
        }

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

[System.Serializable]
public class SerializableCatalystSites
{

    public CatalystSite[] sites;

}