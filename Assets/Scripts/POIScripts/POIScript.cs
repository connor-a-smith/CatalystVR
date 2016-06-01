using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class POIScript : MonoBehaviour {

    bool activated = false;
    bool visible = true;
    List<POIScriptComponent> components;

    public float viewDistance = 10f;


	// Use this for initialization
	void Start () {
        components = new List<POIScriptComponent>(GetComponentsInChildren<POIScriptComponent>());
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Controller.player.transform.position);

        if (Vector3.Distance(Controller.player.transform.position, transform.position) > viewDistance)
        {

        }
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

    public void Activate()
    {
        //If selected another node after without deactivating an old one.
        if (Controller.selectedPOI != null && Controller.selectedPOI != this )
        {
            Controller.selectedPOI.Deactivate();
        }

        activated = true;
        Controller.selectedPOI = this;

        for (int i= 0; i < components.Count; i++)
        {
            components[i].Activate();
        }

    }

    public void Deactivate()
    {
        activated = false;
        Controller.selectedPOI = null;

        for (int i = 0; i < components.Count; i++)
        {
            components[i].Deactivate();
        }
    }

    public void Trigger()
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
}
