using UnityEngine;
using System.Collections;

public class GrabScript : MonoBehaviour {
    //NOTE ALL ARRAYS USE 0 AS LEFT AND 1 AS RIGHT.

    //The controllers, used to check if buttons are pressed and location.
    SixenseHand[] m_hands;

    //The Transforms where grabbing occurs
    Transform[] grabPoints;

    //If an object is being dragged it will be stored here so its parent can be set to null after done.
    Transform[] grabbedObjects;

    //Store the positions of the hands last frame for the velocity when an object is being held.
    Vector3[] lastPositions;

    //How far back the grabbed object appears to look natural.
    public float grabbedObjectOffset = -.05f;


    // Use this for initialization
    void Start() {
        m_hands = GetComponentsInChildren<SixenseHand>();
        grabPoints = new Transform[2];
        grabbedObjects = new Transform[2];
        lastPositions = new Vector3[2];


        for(int i = 0; i < m_hands.Length; i++) {
            //Debug.Log(i);
            grabPoints[i] = m_hands[i].transform.Find("GrabPoint").transform;
            //Debug.Log(grabPoints[i]);
        }
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(m_hands[1].transform.position);
        for(int i = 0; i < m_hands.Length; i++) {
            RaycastHit hit;
            Physics.Raycast(grabPoints[i].position, grabPoints[i].forward, out hit, Mathf.Infinity);


            //If it got something, pull it.
            if(hit.transform != null) {
                grabPoints[i].GetComponent<LineRenderer>().SetColors(Color.green, Color.green);
            }

            else {

                //grabPoints[i].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 5));
                grabPoints[i].GetComponent<LineRenderer>().SetColors(Color.white, Color.white);
            }

            //On release, reset the mover.
            if(IsControllerActive(m_hands[i].m_controller) && m_hands[i].m_controller.GetButtonUp(SixenseButtons.TRIGGER)) {
                if(grabbedObjects[i] != null) {
                    //Potentially lerp mover back?
                    grabbedObjects[i].GetComponent<ShipMovementScript>().returnToStart();

                    grabbedObjects[i] = null;
                }
            }

            if(IsControllerActive(m_hands[i].m_controller) && m_hands[i].m_controller.GetButtonDown(SixenseButtons.TRIGGER)) {
                //RaycastHit hit;
                Physics.Raycast(grabPoints[i].position, grabPoints[i].forward, out hit, Mathf.Infinity);

                GameObject hitObject = Controller.inputManager.HandleHit(hit);

                ShipMovementScript mover = hit.transform.gameObject.GetComponent<ShipMovementScript>();

                if (mover)
                {
                    grabbedObjects[i] = hit.transform;
                    grabbedObjects[i].transform.position = grabPoints[i].transform.position; // new Vector3(0, 0, grabbedObjectOffset);

                    mover.setNewTarget();

                    //Setting local position on the ship, then setting new parent.
                    hit.transform.parent = grabPoints[i];

                }
                //Debug.Log(hit.transform.gameObject.name);
            }

            lastPositions[i] = m_hands[i].transform.localPosition;

        }
    }

    /** returns true if a controller is enabled and not docked */
    bool IsControllerActive(SixenseInput.Controller controller) {
        return (controller != null && controller.Enabled && !controller.Docked);
    }
}