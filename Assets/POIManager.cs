using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class POIManager : MonoBehaviour {

    public static List<POIScript> POIList;

    [SerializeField] public BookmarkController bookmarks;
    [SerializeField] public GameObject bookmarkPOIList;
    [SerializeField] public Object bookmarkPrefab;
    [SerializeField] public GameObject bookmarkPanel;

    public static POIScript selectedPOI;

    public static Material defaultPOIMat;
    public static Material highlightedPOIMat;
    public static Material selectedPOIMat;

    public Material defaultPOIMaterialEditor;
    public Material highlightedPOIMaterialEditor;
    public Material selectedPOIMaterialEditor;

    private void Awake()
    {

        SceneManager.sceneUnloaded += ClearPOIList;
        SceneManager.sceneLoaded += UpdateBookmarks;

        defaultPOIMat = defaultPOIMaterialEditor;
        highlightedPOIMat = highlightedPOIMaterialEditor;
        selectedPOIMat = selectedPOIMaterialEditor;

    }

    public static void AddPOI(POIScript POI)
    {

        if (POIList == null)
        {
            POIList = new List<POIScript>();
        }

        POIList.Add(POI);

    }

    public void UpdateBookmarks(Scene scene, LoadSceneMode mode)
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

    public void ClearPOIList(Scene scene)
    {

        POIList.Clear();

        bookmarks.ClearBookmarks();

    }
}
