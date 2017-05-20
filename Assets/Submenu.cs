using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submenu : MonoBehaviour {

    private static GameObject submenuObject;

    void Awake ()
    {
        if (submenuObject == null)
        {
            submenuObject = gameObject;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Deactivate()
    {
        submenuObject.SetActive(false);
        Debug.LogWarning("Deactivate called");
    }
}
