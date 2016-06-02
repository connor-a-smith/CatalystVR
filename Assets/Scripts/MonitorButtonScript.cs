using UnityEngine;
using System.Collections;

public class MonitorButtonScript : MonoBehaviour {
    public ComponentType specificType;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Toggle()
    {
        //connectedComponent.Toggle();
    }

    void OnMouseDown()
    {
        Toggle();
    }
}
