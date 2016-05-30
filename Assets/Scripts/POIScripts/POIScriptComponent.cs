using UnityEngine;
using System.Collections;

public abstract class POIScriptComponent : MonoBehaviour {

    bool activated;

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
}
