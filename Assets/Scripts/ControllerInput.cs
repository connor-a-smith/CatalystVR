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

    private MonitorButtonScript currentlySelectedButton = null;
    private int selectedButtonIndex = 0;

    public Camera raycastCam;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("RightStickHorizontal") != 0)
        {
            //Controller.playerShip.transform.Rotate(0, Time.deltaTime * Input.GetAxis("RightStickHorizontal") * xRotationSpeed, 0, Space.Self, );
            Controller.playerShip.transform.RotateAround(raycastCam.transform.parent.position, Vector3.up, Time.deltaTime * Input.GetAxis("RightStickHorizontal") * xRotationSpeed);

        }

        if (Input.GetAxis("RightStickVertical") != 0)
        {
            Controller.playerShip.transform.Rotate(Time.deltaTime * Input.GetAxis("RightStickVertical") * yRotationSpeed, 0, 0, Space.Self);
            //Controller.playerShip.transform.RotateAround(raycastCam.transform.parent.transform.position, Vector3.left, Time.deltaTime * Input.GetAxis("RightStickVertical") * yRotationSpeed);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("Horizontal") * forwardSpeed * Controller.playerShip.transform.right);
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("Vertical") * forwardSpeed * Controller.playerShip.transform.forward);
        }

        if (Input.GetAxis("Xbox DpadX") != 0 && !justActivatedDpad)
        {
            //Deselect the old button.
            Controller.buttons[selectedButtonIndex].deselect();
            
            if (Controller.selectedPOI != null)
            {
                //Increment the button index while the button is not active. Loops due to the mod. Guaranteed to be at least one active button, the back button.
                //Adding the length of buttons in case index is 0 and moving left. Then -1 becomes length -1, going to end. All else will be handled by mod.
                do
                {
                    selectedButtonIndex = (selectedButtonIndex + (int)Input.GetAxis("Xbox DpadX") + Controller.buttons.Length) % Controller.buttons.Length;
                }
                while (!Controller.buttons[selectedButtonIndex].activatable);

                //Select this new button.
                Controller.buttons[selectedButtonIndex].select();
                justActivatedDpad = true;
            }
        }

        //On release of dpad, allow to be used again.
        if (Input.GetAxis("Xbox DpadX") == 0 && justActivatedDpad)
        {
            justActivatedDpad = false;
        }

        //On a press, activate
        if (Input.GetAxis("Xbox A") != 0 && !justActivatedA)
        {
            justActivatedA = true;

            if (Controller.selectedPOI == null)
            {
                RaycastHit hit;
                Physics.Raycast(raycastCam.transform.position, raycastCam.transform.forward, out hit, Mathf.Infinity);

                Controller.inputManager.HandleHit(hit);

                //If a POI was selected by this hit, we need to set the active GUI object.
                if (Controller.selectedPOI != null)
                {
                    //Increment the button index while the button is not active. Loops due to the mod. Guaranteed to be at least one active button, the back button.
                    //Similar to loop in dpad, but if current button is active, then there is no change.
                    while (!Controller.buttons[selectedButtonIndex].activatable)
                    {
                        selectedButtonIndex = (selectedButtonIndex + 1) % Controller.buttons.Length;
                    }

                    //Select this new button.
                    Controller.buttons[selectedButtonIndex].select();
                }
            }

            //POI Selected, trigger it.
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
}
