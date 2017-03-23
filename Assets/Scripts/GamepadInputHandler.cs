using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GamepadInputHandler : MonoBehaviour {

    private int POILayerMask;

    /*
    // Use this for initialization
    void Start() {

        //Set raycast to hit everything except Ignore Raycast.
        POILayerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
    }

    // Update is called once per frame
    void Update() {

        if(Input.GetAxis("RightStickHorizontal") != 0) {

            //POI selected and not advanced mode, stick controller POI movement.
            else if(!justActivatedRightStickHorizontal) {

                else {

    if(Input.GetAxis("RightStickHorizontal") > 0) {
                        pickActiveButton(1);
                    }
                    else {
                        pickActiveButton(-1);
                    }
                }
            }
        }

        if(Input.GetAxis("Horizontal") != 0) {

                else {


                    //Debug.Log(Input.GetAxis("RightStickHorizontal"));
                    if(Input.GetAxis("Horizontal") > 0) {
                        pickActiveButton(1);
                    }
                    else {
                        pickActiveButton(-1);
                    }
                }
            }
        }

        //On a press, activate
        if(Input.GetAxis("Xbox A") != 0 && !justActivatedA) {

            if(GameManager.instance.bookmarks.bookmarkPanelActivated) {

                pickActiveButton(0);

            }

            //If no POI selected, then try to select a new POI
            else if(POIManager.selectedPOI == null) {
                RaycastHit hit;
                Physics.Raycast(GameManager.instance.raycastCam.transform.position, GameManager.instance.raycastCam.transform.forward, out hit, Mathf.Infinity, POILayerMask, QueryTriggerInteraction.Collide);
                GameManager.inputManager.HandleHit(hit);


                //If a POI was selected by this hit, we need to set the active GUI object.
                if(POIManager.selectedPOI != null) {
                    pickActiveButton(0);
                }
            }


            //POI Selected, trigger its buttons.
            else {
                GameManager.buttons[selectedButtonIndex].AttemptToggle();
            }
        }

        //On release of button, allow to be used again.
        else if(Input.GetAxis("Xbox A") == 0) {
            justActivatedA = false;
        }

        //On b press, deactvate selected POI.
        if(Input.GetAxis("Xbox B") != 0 && !justActivatedA) {

            timeSinceLastInput = 0.0f;

            if(POIManager.selectedPOI != null) {
                POIManager.selectedPOI.Deactivate();
            }
        }


    }

    /// <summary>
    /// Sets the selectedButtonIndex to the desired value, checking for active buttons.
    /// </summary>
    void pickActiveButton(int offset) {
        //Saving this value to detect infinite loops.
        int originalPosition = selectedButtonIndex;

        //Deselect the old button.
        GameManager.buttons[selectedButtonIndex].deselect();

        //Increment the button index while the button is not active. Loops due to the mod. Guaranteed to be at least one active button, the back button.
        //Adding the length of buttons in case index is 0 and moving left. Then -1 becomes length -1, going to end. All else will be handled by mod.
        do {
            selectedButtonIndex = (selectedButtonIndex + offset + GameManager.buttons.Length) % GameManager.buttons.Length;

            //If trying to get the first available button, offset is 0 to check current button, then 1 to check others.
            if(offset == 0) {
                offset = 1;
            }

            //Emergency situation, no buttons are selected and we made a complete loop. Just return.
            else if(originalPosition == selectedButtonIndex) {
                return;
            }
        } while(!GameManager.buttons[selectedButtonIndex].activatable);

        //Select this new button.
        GameManager.buttons[selectedButtonIndex].select();
    }


    */
}
