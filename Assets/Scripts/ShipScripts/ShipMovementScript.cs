using UnityEngine;
using System.Collections;

public class ShipMovementScript : MonoBehaviour {

    //Returns here when done.
    public Vector3 startPoint;
    public Vector3 startRotation;

    //The current deltas to use.
    public Vector3 currentStartPoint;
    public Vector3 currentStartRotation;
    
    public float threshold = 0.1f;
    public float transformScale = 5.0f;
    public float rotateScale = 1.0f;

    // Use this for initialization
    void Start () {
        startPoint = transform.localPosition;
        startRotation = transform.localRotation.eulerAngles;

        currentStartPoint = startPoint;
        currentStartRotation = startRotation;
	}
	
	// Update is called once per frame
	void Update () {


        Vector3 deltaPosition = transform.localPosition - currentStartPoint;
        Vector3 deltaRotation = transform.localRotation.eulerAngles - currentStartRotation;

        if (Vector3.Magnitude(deltaPosition) > 0)
        {
            //Note moving in self space to account for rotations.
            Controller.playerShip.transform.Translate(deltaPosition * transformScale * Time.deltaTime, Space.Self);
        }

        if (Vector3.Magnitude(deltaRotation) > 0)
        {
            //Change this to modify other axis too. Currently only changing y axis.
            Controller.playerShip.transform.Rotate(new Vector3(0, deltaRotation.y, 0) * rotateScale * Time.deltaTime);
        }
    }

    public void setNewCurrentStart()
    {
        currentStartPoint = transform.localPosition;
        currentStartRotation = transform.localRotation.eulerAngles;
    }

    public void returnToStart()
    {
        transform.parent = Controller.playerShip.transform;

        transform.localPosition = startPoint;
        transform.localRotation = Quaternion.Euler(startRotation);

   }
}
