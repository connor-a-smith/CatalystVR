using UnityEngine;
using System.Collections;

public class LabelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + GameManager.instance.raycastCam.transform.rotation * Vector3.forward,
        GameManager.instance.raycastCam.transform.rotation * Vector3.up);
    }
}
