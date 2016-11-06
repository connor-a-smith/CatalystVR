using UnityEngine;
using System.Collections;

public class FocusTransformComponent : POIScriptComponent
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate()
    {
        base.Activate();

        //Want the raycast cam to look at the POI. So we get the rotation needed for the cam to see it and we apply to the entire platform.
        Quaternion oldRot = Controller.instance.raycastCam.transform.localRotation;
        Controller.instance.raycastCam.transform.LookAt(this.transform.position);
        Quaternion newRot = Controller.instance.raycastCam.transform.localRotation;
        Debug.Log(Controller.instance.raycastCam.transform.rotation.eulerAngles + " Desired");

        //Vector3 oldRot = Controller.instance.raycastCam.transform.localEulerAngles;
        //Controller.instance.raycastCam.transform.LookAt(this.transform.position);
        //Vector3 newRot = Controller.instance.raycastCam.transform.localEulerAngles;

        //Apply to platform, and reset raycastCam pos.

        Controller.playerShip.transform.rotation *= ((newRot * Quaternion.Inverse(oldRot)));
        //Controller.playerShip.transform.Rotate(newRot - oldRot);
        Controller.instance.raycastCam.transform.localRotation = oldRot;

        Debug.Log(Controller.instance.raycastCam.transform.rotation.eulerAngles + " Actual");


        //Controller.playerShip.transform.position = this.transform.position - Controller.playerShip.transform.forward * 50.0f;

        //Teleport to location.


        //Vector3 newRotation = Controller.playerShip.transform.rotation.eulerAngles;
        //newRotation.z = 0;
        //Controller.playerShip.transform.rotation = Quaternion.Euler(newRotation);

        //Controller.playerShip.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //Lerp to location. Need to implement

        //Accelerate to location. Need to implement.
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
