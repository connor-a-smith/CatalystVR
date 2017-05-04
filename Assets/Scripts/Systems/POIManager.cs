using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class POIManager : MonoBehaviour {

    public static List<POI> POIList;

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

        SceneManager.sceneUnloaded += ClearPOIList;
        SceneManager.sceneLoaded += UpdateBookmarks;

        defaultPOIMat = defaultPOIMaterialEditor;
        highlightedPOIMat = highlightedPOIMaterialEditor;
        selectedPOIMat = selectedPOIMaterialEditor;

        gameManager = GetComponentInParent<GameManager>();
        bookmarks = gameManager.user.GetComponentInChildren<BookmarkController>();

    }

    public void Start()
    {

        //   CreateNewPOI(32.8801f, 117.2340f);

       // LatLonTest newTest = new LatLonTest();
       // string jsonText = JsonUtility.ToJson(newTest);
       // File.WriteAllText("testa.json", jsonText);

      string readJSON = File.ReadAllText("latlonsample.json");

      LatLonTest loadedLatLon = JsonUtility.FromJson<LatLonTest>(readJSON);

        foreach (LatLonTest.location loc in loadedLatLon.latlons) {


            CreateNewPOI(loc.latitude, loc.longitude, loc.state);

         }

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

    public POI CreateNewPOI(float latitude, float longitude, string name)
    {

        Vector3 pos = CatalystEarth.Get3DPositionFromLatLon(latitude, longitude);

        Debug.LogFormat("Getting 3D Position of {0}, {1}: ({2}, {3}, {4})", latitude, longitude, pos.x, pos.y, pos.z);

        GameObject newObj = GameObject.Instantiate(poiPrefab, pos, Quaternion.identity) as GameObject;
        POI newPOI = newObj.GetComponent<POI>();
        newPOI.POIName = name;

        newObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up) * Quaternion.LookRotation(CatalystEarth.earthTransform.position);

        newObj.transform.LookAt(CatalystEarth.earthTransform.position);

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
public class LatLonTest
{

    [System.Serializable]
    public struct location
    {

        public string state;
        public float latitude;
        public float longitude;

        public location (string name, float lat, float lon)
        {

            state = name;
            latitude = lat;
            longitude = lon;
        }

    }

    public location[] latlons;

    public LatLonTest()
    {


        location test1 = new location("test1", 1.0f, 2.0f);
        location test2 = new location("test2", 2.0f, 5.0f);

        latlons = new location[] { test1, test2 };

    }




}
