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
        /*
        //Want the raycast cam to look at the POI. So we get the rotation needed for the cam to see it and we apply to the entire platform.
        Quaternion oldRot = Controller.instance.raycastCam.transform.localRotation;
        Controller.instance.raycastCam.transform.LookAt(this.transform.position);
        Quaternion newRot = Controller.instance.raycastCam.transform.localRotation;

        //Apply to platform, and reset raycastCam pos.

        Controller.playerShip.transform.rotation *= ((newRot * Quaternion.Inverse(oldRot)));
        Controller.instance.raycastCam.transform.localRotation = oldRot;
        
        //
        Controller.playerShip.transform.position = this.transform.position - Controller.instance.raycastCam.transform.forward * 50.0f;
        */
        //Teleport to location.

        GameManager.playerShip.transform.position = this.transform.position + 50 * this.transform.forward + 50 * this.transform.up;

        Quaternion oldRot = GameManager.instance.raycastCam.transform.localRotation;
        GameManager.instance.raycastCam.transform.LookAt(this.transform.position);
        Quaternion newRot = GameManager.instance.raycastCam.transform.localRotation;

        //Apply to platform, and reset raycastCam pos.

        GameManager.playerShip.transform.rotation *= ((newRot * Quaternion.Inverse(oldRot)));
        GameManager.instance.raycastCam.transform.localRotation = oldRot;
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
