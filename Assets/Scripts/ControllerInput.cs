using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ControllerInput : MonoBehaviour
{
    public float xRotationSpeed = 30;
    public float yRotationSpeed = -30;

    public float forwardSpeed = 5;

    //Vars to Avoid duplicate actions on one press.
    //Was the A button just hit? 
    private bool justActivatedA = false;

    //Was the B button just hit? 
    private bool justActivatedB = false;

    //Was the DPad just used?
    private bool justActivatedDpad = false;

    private bool justActivatedStart = false;

    private bool justActivatedRightStickVertical = false;
    private bool justActivatedRightStickHorizontal = false;
    private bool justActivatedLeftStickVertical = false;
    private bool justActivatedLeftStickHorizontal = false;

    private MonitorButtonScript currentlySelectedButton = null;
    private int selectedButtonIndex = 0;

    //Allows movement while selecting a POI using the Dpad.
    private bool advancedMode = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("RightStickHorizontal") != 0)
        {
            //Movement allowed, no POI selected or advanced mode.
            if (Controller.selectedPOI == null || advancedMode)
            {
                //Controller.playerShip.transform.Rotate(0, Time.deltaTime * Input.GetAxis("RightStickHorizontal") * xRotationSpeed, 0, Space.Self, );
                Controller.playerShip.transform.RotateAround(Controller.instance.raycastCam.transform.parent.position, Vector3.up, Time.deltaTime * Input.GetAxis("RightStickHorizontal") * xRotationSpeed);
            }

            //POI selected and not advanced mode, stick controller POI movement.
            else if (!justActivatedRightStickHorizontal)
            {
                justActivatedRightStickHorizontal = true;
                //Debug.Log(Input.GetAxis("RightStickHorizontal"));
                if (Input.GetAxis("RightStickHorizontal") > 0)
                {
                    pickActiveButton(1);
                }

                else
                {
                    pickActiveButton(-1);
                }

            }
        }

        //On release of stick, allow to be used again.
        else if (Input.GetAxis("RightStickHorizontal") == 0 && justActivatedRightStickHorizontal)
        {
            justActivatedRightStickHorizontal = false;
        }

        if (Input.GetAxis("RightStickVertical") != 0)
        {
            if (Controller.selectedPOI == null || advancedMode)
            {
                Controller.playerShip.transform.Rotate(Time.deltaTime * Input.GetAxis("RightStickVertical") * yRotationSpeed, 0, 0, Space.Self);
                //Controller.playerShip.transform.RotateAround(raycastCam.transform.parent.transform.position, Vector3.left, Time.deltaTime * Input.GetAxis("RightStickVertical") * yRotationSpeed);
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            //Movement allowed, no POI selected or advanced mode.
            if (Controller.selectedPOI == null || advancedMode)
            {
                Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("Horizontal") * forwardSpeed * Controller.playerShip.transform.right);
            }



            //POI selected and not advanced mode, stick controller POI movement.
            else if (!justActivatedLeftStickHorizontal)
            {
                justActivatedLeftStickHorizontal = true;
                //Debug.Log(Input.GetAxis("RightStickHorizontal"));
                if (Input.GetAxis("Horizontal") > 0)
                {
                    pickActiveButton(1);
                }

                else
                {
                    pickActiveButton(-1);
                }
            }
        }

        //On release of stick, allow to be used again.
        else if (Input.GetAxis("Horizontal") == 0 && justActivatedLeftStickHorizontal)
        {
            justActivatedLeftStickHorizontal = false;
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            if (Controller.selectedPOI == null || advancedMode)
            {
                Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("Vertical") * forwardSpeed * Controller.playerShip.transform.forward);
            }
        }

        if (Input.GetAxis("Xbox DpadX") != 0 && !justActivatedDpad)
        {
            if (Controller.selectedPOI != null)
            {
                pickActiveButton((int)Input.GetAxis("Xbox DpadX"));
                justActivatedDpad = true;
            }
        }

        //On release of dpad, allow to be used again.
        else if (Input.GetAxis("Xbox DpadX") == 0 && justActivatedDpad)
        {
            justActivatedDpad = false;
        }

        //On a press, activate
        if (Input.GetAxis("Xbox A") != 0 && !justActivatedA)
        {
            justActivatedA = true;

            //If no POI selected, then try to select a new POI
            if (Controller.selectedPOI == null)
            {
                RaycastHit hit;
                Physics.Raycast(Controller.instance.raycastCam.transform.position, Controller.instance.raycastCam.transform.forward, out hit, Mathf.Infinity);

                Controller.inputManager.HandleHit(hit);

                //If a POI was selected by this hit, we need to set the active GUI object.
                if (Controller.selectedPOI != null)
                {
                    pickActiveButton(0);
                }
            }

            //POI Selected, trigger its buttons.
            else
            {
                Controller.buttons[selectedButtonIndex].AttemptToggle();
            }
        }

        //On release of button, allow to be used again.
        else if (Input.GetAxis("Xbox A") == 0)
        {
            justActivatedA = false;
        }

        //On b press, deactvate selected POI.
        if (Input.GetAxis("Xbox B") != 0 && !justActivatedA)
        {
            if (Controller.selectedPOI != null)
            {
                Controller.selectedPOI.Deactivate();
            }
        }

        //On release of button, allow to be used again.
        else if (Input.GetAxis("Xbox B") == 0)
        {
            justActivatedB = false;
        }

        if (Input.GetAxis("Xbox Start") == 1)
        {
            justActivatedStart = true;
            SceneManager.LoadScene("MultiDisplayPlanet");
        }

        else if (Input.GetAxis("Xbox Start") == 0)
        {
            justActivatedStart = false;
        }


        //TODO Remove
        if (Input.GetKey(KeyCode.A))//else if (Input.GetButton("joystick button 0"))
        {
            SceneManager.LoadScene("Mar Saba");
        }
        if (Input.GetKey(KeyCode.S))//else if (Input.GetButton("joystick button 0"))
        {
            SceneManager.LoadScene("Luxor");
        }
        if (Input.GetKey(KeyCode.D))//else if (Input.GetButton("joystick button 0"))
        {
            SceneManager.LoadScene("MultiDisplayPlanet");
        }
    }

    /// <summary>
    /// Sets the selectedButtonIndex to the desired value, checking for active buttons.
    /// </summary>
    void pickActiveButton(int offset)
    {
        //Saving this value to detect infinite loops.
        int originalPosition = selectedButtonIndex;

        //Deselect the old button.
        Controller.buttons[selectedButtonIndex].deselect();

        //Increment the button index while the button is not active. Loops due to the mod. Guaranteed to be at least one active button, the back button.
        //Adding the length of buttons in case index is 0 and moving left. Then -1 becomes length -1, going to end. All else will be handled by mod.
        do
        {
            selectedButtonIndex = (selectedButtonIndex + offset + Controller.buttons.Length) % Controller.buttons.Length;

            //If trying to get the first available button, offset is 0 to check current button, then 1 to check others.
            if (offset == 0)
            {
                offset = 1;
            }

            //Emergency situation, no buttons are selected and we made a complete loop. Just return.
            else if (originalPosition == selectedButtonIndex)
            {
                return;
            }
        }
        while (!Controller.buttons[selectedButtonIndex].activatable);

        //Select this new button.
        Controller.buttons[selectedButtonIndex].select();
    }
}
