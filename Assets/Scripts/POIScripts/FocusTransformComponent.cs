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

        //Teleport to location. Shifting down by cam height so that camera is in the correct position.
        Controller.playerShip.transform.position = this.transform.position;
        Controller.playerShip.transform.LookAt(this.gameObject.transform);

        Vector3 newRotation = Controller.playerShip.transform.rotation.eulerAngles;
        newRotation.z = 0;
        Controller.playerShip.transform.rotation = Quaternion.Euler(newRotation);

        Controller.playerShip.transform.position += (-Controller.playerShip.transform.forward * 50.0f);
        Controller.playerShip.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //Lerp to location. Need to implement

        //Accelerate to location. Need to implement.
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
