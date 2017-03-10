using UnityEngine;
using System.Collections;

public class ShipMovementScript : MonoBehaviour {

    //Returns here when done.
    public Vector3 startPoint;
    public Vector3 startRotation;

    //The current deltas to use.
    public Vector3 targetLocalPoint;
    public Vector3 targetLocalRotation;
    
    public float threshold = 0.1f;
    public float transformScale = 5.0f;
    public float rotateScale = 1.0f;

    // Use this for initialization
    void Start () {
        startPoint = transform.localPosition;
        startRotation = transform.localRotation.eulerAngles;

        targetLocalPoint = startPoint;
        targetLocalRotation = startRotation;
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 targetPoint = Controller.playerShip.transform.position + targetLocalPoint;
        // Vector3 targetRotation = Controller.playerShip.transform.rotation.eulerAngles + targetLocalRotation;

        //Debug.Log("t" + targetLocalPoint);
        //Debug.Log(targetPoint);
        //Debug.Log(transform.position);

        //Want to get local values relative to old parent. Thus resetting parent for the local values.

        Transform oldParent = transform.parent;
        transform.parent = GameManager.playerShip.transform;

        Vector3 deltaPosition = transform.localPosition - targetLocalPoint;
        Vector3 deltaRotation = transform.localRotation.eulerAngles - targetLocalRotation;

        //Setting angles of 180-360 to their -180-0 counterparts.
        if (deltaRotation.x > 180)
        {
            deltaRotation.x -= 360;
        }

        if (deltaRotation.y > 180)
        {
            deltaRotation.y -= 360;
        }

        if (deltaRotation.z > 180)
        {
            deltaRotation.z -= 360;
        }

        transform.parent = oldParent;

        //Debug.Log(deltaPosition);


        if (Vector3.Magnitude(deltaPosition) > threshold)
        {
            //Note moving in self space to account for rotations.
            GameManager.playerShip.transform.Translate(deltaPosition * transformScale * Time.deltaTime, Space.Self);
        }

        if (Vector3.Magnitude(deltaRotation) > threshold)
        {
            //Change this to modify other axis too. Currently only changing y axis.
            GameManager.playerShip.transform.Rotate(new Vector3(0, deltaRotation.y, 0) * rotateScale * Time.deltaTime);
        }
    }

    public void setNewTarget()
    {
        targetLocalPoint = transform.localPosition;
        targetLocalRotation = transform.localRotation.eulerAngles;
    }

    public void returnToStart()
    {
        transform.parent = GameManager.playerShip.transform;

        transform.localPosition = startPoint;
        transform.localRotation = Quaternion.Euler(startRotation);

        targetLocalPoint = transform.localPosition;
        targetLocalRotation = transform.localRotation.eulerAngles;

   }
}
