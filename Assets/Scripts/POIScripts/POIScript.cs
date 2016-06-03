using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class POIScript : MonoBehaviour {

    bool activated = false;
    List<POIScriptComponent> components;

	// Use this for initialization
	void Start () {
        components = new List<POIScriptComponent>(GetComponentsInChildren<POIScriptComponent>());
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Controller.playerShip.transform.position);
	}

    void OnMouseDown()
    {
        Toggle();
    }

    public void Activate()
    {
        //If selected another node after without deactivating an old one.
        if (Controller.selectedPOI != null && Controller.selectedPOI != this && Controller.selectedPOI)
        {
            Controller.selectedPOI.Deactivate();
        }

        activated = true;
        Controller.selectedPOI = this;


        MonitorButtonScript[] buttons = Controller.monitor.GetComponentsInChildren<MonitorButtonScript>();

        //Tell all the buttons that a new poi was selected.
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].OnNewNodeSelected();
        }

        //Tell all components to activate.
        for (int i= 0; i < components.Count; i++)
        {
            if (components[i].activateImmediately)
            {
                components[i].Activate();
            }
        }

    }

    public void Deactivate()
    {
        activated = false;
        Controller.selectedPOI = null;

        MonitorButtonScript[] buttons = Controller.monitor.GetComponentsInChildren<MonitorButtonScript>();

        //Tell all the buttons that there is no current POI.
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].OnNodeDeselected();
        }

        for (int i = 0; i < components.Count; i++)
        {
            components[i].Deactivate();
        }
    }

    /// <summary>
    /// A way to toggle the state without needing to check the current state externally.
    /// </summary>
    public void Toggle()
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
