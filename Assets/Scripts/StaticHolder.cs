using UnityEngine;
using System.Collections;

public class StaticHolder : MonoBehaviour {

    public static GameObject player;

	// Use this for initialization
	void Start () {
        player = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
