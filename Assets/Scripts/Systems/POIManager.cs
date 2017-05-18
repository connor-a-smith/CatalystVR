using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class POIManager : MonoBehaviour {

    public static List<POI> POIList;

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
            //BookmarkController.submenuRect.SetActive(false);

        }

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
