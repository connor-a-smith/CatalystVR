using UnityEngine;
using System.Collections;

public class MonitorDeactivateButtonScript : MonitorButtonScript {

    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Toggle if activatable.
    /// </summary>
    public override void AttemptToggle()
    {
        if (activatable)
        {
            Controller.selectedPOI.Deactivate();
        }
    }

    public override void OnNewNodeSelected()
    {
            activatable = true;
    }

    public override void OnNodeDeselected()
    {
        activatable = false;
    }
}
