using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public static GameObject player;
    public static MonitorScript monitor;

    public static POIScript selectedPOI;

    public static Material defaultPOIMat;
    public static Material highlightedPOIMat;
    public static Material selectedPOIMat;

    public Material defaultPOIMaterialEditor;
    public Material highlightedPOIMaterialEditor;
    public Material selectedPOIMaterialEditor;


    // Use this for initialization
    void Start () {
        player = this.gameObject;

        defaultPOIMat = defaultPOIMaterialEditor;
        highlightedPOIMat = highlightedPOIMaterialEditor;
        selectedPOIMat = selectedPOIMaterialEditor;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
