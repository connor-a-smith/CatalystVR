using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class POIScript : MonoBehaviour {

    bool activated = false;
    List<POIScriptComponent> components;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(StaticHolder.player.transform.position);
	}

    void OnMouseDown()
    {
        if (!activated)
        {
            Activate();
        }

        else
        {
            Deactivate();
        }
    }

    void Activate()
    {
        activated = true;
        for (int i= 0; i < components.Count; i++)
        {
            components[i].Activate();
        }

    }

    void Deactivate()
    {
        activated = false;
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Deactivate();
        }
    }
}
