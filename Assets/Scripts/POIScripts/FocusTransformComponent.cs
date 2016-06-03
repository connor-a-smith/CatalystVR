using UnityEngine;
using System.Collections;

public class FocusTransformComponent : POIScriptComponent {

    public GameObject transformValueObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();

        Debug.Log(transformValueObject.transform.position); 

        //Teleport to location.
        Controller.playerShip.transform.position = transformValueObject.transform.position;
        Controller.playerShip.transform.rotation = transformValueObject.transform.rotation;

        //Lerp to location.

        //Accelerate to location.
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
