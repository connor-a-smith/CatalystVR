using UnityEngine;
using System.Collections;

public class CAVERenderScript : MonoBehaviour {

    Camera thisCam;
    // Use this for initialization
    void Start () {
        thisCam = GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void LateUpdate () {
        //thisCam.projectionMatrix;
	}
}
