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

        //Teleport to location. Shifting down by cam height so that camera is in the correct position.
        Controller.playerShip.transform.position = transformValueObject.transform.position - new Vector3(0,Camera.main.gameObject.transform.localPosition.y,0);

        //Use only y component.
        Vector3 angles = transformValueObject.transform.rotation.eulerAngles;

        angles.x = 0;
        angles.z = 0;

        Controller.playerShip.transform.rotation = Quaternion.Euler(angles);

        //Lerp to location. Need to implement

        //Accelerate to location. Need to implement.
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
