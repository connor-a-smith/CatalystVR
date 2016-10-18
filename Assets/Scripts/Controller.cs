using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

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

    public static MonitorButtonScript[] buttons;

    public static string instructionText = "Select a POI or grab the sphere to move. The further the sphere from the grab point, the faster you'll go in the direction you moved it.";

    // Use this for initialization
    /// <summary>
    /// Used to set up the static variables based on the dragged in variables.
    /// </summary>
    void Awake () {
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

        Controller.monitor.GetComponentInChildren<Text>().text = Controller.instructionText;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
