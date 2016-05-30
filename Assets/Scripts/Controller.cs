using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public static GameObject player;
    public static MonitorScript monitor;

    public static POIScript selectedPOI;

	// Use this for initialization
	void Start () {
        player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
