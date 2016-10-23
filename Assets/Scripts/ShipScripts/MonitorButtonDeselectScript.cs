using UnityEngine;
using System.Collections;

public class MonitorButtonDeselectScript : POIScriptComponent {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();
        Controller.selectedPOI.Deactivate();

    }
}
