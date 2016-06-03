using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public static GameObject playerShip;
    public static MonitorScript monitor;

    public static POIScript selectedPOI;

    public static Material defaultPOIMat;
    public static Material highlightedPOIMat;
    public static Material selectedPOIMat;

    public Material defaultPOIMaterialEditor;
    public Material highlightedPOIMaterialEditor;
    public Material selectedPOIMaterialEditor;

    public static Object photoPrefab;
    public Object photoPrefabEditor;


    // Use this for initialization
    /// <summary>
    /// Used to set up the static variables based on the dragged in variables.
    /// </summary>
    void Awake () {
        playerShip = this.gameObject;

        defaultPOIMat = defaultPOIMaterialEditor;
        highlightedPOIMat = highlightedPOIMaterialEditor;
        selectedPOIMat = selectedPOIMaterialEditor;

        photoPrefab = photoPrefabEditor;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
