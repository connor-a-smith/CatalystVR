using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour
{

    CatalystPhoto selectedPhoto;

    // Use this for initialization
    void Start()
    {
        string[] controllerNames = Input.GetJoystickNames();
        for (int i = 0; i < controllerNames.Length; i++)
        {
            if (controllerNames[i].Contains("XBOX"))
            {
                //If this is the controller, then set all gamepadObjects active.
                GetComponentInChildren<ControllerInput>(true).transform.parent.gameObject.SetActive(true);
                Controller.isCave = true;
            }

            //TODO test with hydra
            else if (controllerNames[i].Contains("hydra"))
            {
                //Activate appropriate hydra components.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Handle hit does things that happen for all control schemes then returns the object so that device specific actions may be performed.
    /// </summary>
    /// <param name="hit">The output parameter from raycast.</param>
    /// <returns></returns>
    public GameObject HandleHit(RaycastHit hit)
    {
        //If it got something, activate it.
        if (hit.transform != null)
        {
            POIScript poi = hit.transform.gameObject.GetComponent<POIScript>();

            if (poi)
            {
                poi.Toggle();
            }

            MonitorButtonScript button = hit.transform.gameObject.GetComponent<MonitorButtonScript>();

            if (button)
            {
                button.AttemptToggle();
            }

            CatalystPhoto hitPhoto = hit.collider.gameObject.GetComponent<CatalystPhoto>();

            if (hitPhoto)
            {
                //if a photo has already been selected and is on screen
                if (selectedPhoto)
                {
                    //transition the selected photo away
                    selectedPhoto.ImageTransition();
                }

                //if the user clicked the same photo they had selected before
                if (hitPhoto == selectedPhoto)
                {

                    //deselects the photo
                    selectedPhoto = null;
                }

                //if the user clicked a different photo
                else {
                    //sets the selected photo to the newly hit one
                    selectedPhoto = hitPhoto;
                    selectedPhoto.ImageTransition();
                }
            }

            HomeButtonScript home = hit.collider.gameObject.GetComponent<HomeButtonScript>();

            if (home)
            {
                home.GoHome();
            }

            return hit.transform.gameObject;
        }
        return null;
    }
}
