﻿using UnityEngine;
using System.Collections;

public class MonitorButtonDeselectScript : POIComponent {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate(GameManager gameManager, CatalystSite associatedSite)
    {
        //Does not need to worry about base.activate, as soon as this component is activated, the entire POI is deactivated.
        //base.Activate();
        POIManager.selectedPOI.Deactivate();
    }
}
