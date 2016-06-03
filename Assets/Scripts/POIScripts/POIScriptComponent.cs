using UnityEngine;
using System.Collections;

public enum ComponentType {Photos, Audio, Videos, Text, Scene, Zoom}

public abstract class POIScriptComponent : MonoBehaviour {

    bool activated;
    public bool activateImmediately = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Activate()
    {
        activated = true;
    }

    public virtual void Deactivate()
    {
        activated = false;
    }

    /// <summary>
    /// Toggles the current state.
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
