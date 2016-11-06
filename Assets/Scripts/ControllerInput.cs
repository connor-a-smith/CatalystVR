using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ControllerInput : MonoBehaviour
{

    [SerializeField] private float maxTiltAngle = 45.0f;

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


    private bool justActivatedRightStickVertical = false;
    private bool justActivatedRightStickHorizontal = false;
    private bool justActivatedLeftStickVertical = false;
    private bool justActivatedLeftStickHorizontal = false;
    private bool justActivatedStart = false;
    private bool justActivatedBack = false;

    private MonitorButtonScript currentlySelectedButton = null;
    private int selectedButtonIndex = 0;

    private bool verticalDown = false;

    //Allows movement while selecting a POI using the Dpad.
    private bool advancedMode = false;


    private int POILayerMask;
    
    // Use this for initialization
    void Start()
    {
        //Set raycast to hit everything except Ignore Raycast.
        POILayerMask = ~(1<<LayerMask.NameToLayer("Ignore Raycast"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("RightStickHorizontal") != 0)
        {
            //Movement allowed, no POI selected or advanced mode.
            if ((Controller.selectedPOI == null && !Controller.instance.bookmarks.bookmarkPanelActivated) || advancedMode)
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
            if ((Controller.selectedPOI == null && !Controller.instance.bookmarks.bookmarkPanelActivated) || advancedMode)
            {

                Transform shipTransform = Controller.playerShip.transform;

                Vector3 beforeShipRotation = shipTransform.localRotation.eulerAngles;

                float rotateAngle = Time.deltaTime * Input.GetAxis("RightStickVertical") * yRotationSpeed;

                shipTransform.Rotate(rotateAngle, 0, 0, Space.Self);

                Vector3 afterShipRotation = shipTransform.localRotation.eulerAngles;

                if(afterShipRotation.x > 180.0f) {

                    afterShipRotation.x -= 360.0f;

                }

                if(afterShipRotation.x < -maxTiltAngle) {

                    afterShipRotation.x = -maxTiltAngle;


                }
                else if(afterShipRotation.x > maxTiltAngle) {

                    afterShipRotation.x = maxTiltAngle;

                }

                afterShipRotation.z = 0.0f;
                shipTransform.localRotation = Quaternion.Euler(afterShipRotation);
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            //Movement allowed, no POI selected or advanced mode.
            if ((Controller.selectedPOI == null  && !Controller.instance.bookmarks.bookmarkPanelActivated) || advancedMode)
            {
                Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("Horizontal") * forwardSpeed * Controller.playerShip.transform.right);
            }

            //POI selected and not advanced mode, stick controller POI movement.
            else if (!Controller.instance.bookmarks.bookmarkPanelActivated && !justActivatedLeftStickHorizontal)
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

            if ((Controller.selectedPOI == null && !Controller.instance.bookmarks.bookmarkPanelActivated) || advancedMode)
            {
                Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("Vertical") * forwardSpeed * Controller.playerShip.transform.forward);
            }

            else if (Controller.instance.bookmarks.bookmarkPanelActivated && !verticalDown) {

                verticalDown = true;

                Debug.LogWarning("HERE");

                if (Input.GetAxis("Vertical") < 0) {

                    Controller.instance.bookmarks.MoveSelectorDown();


                }

                else {

                    Controller.instance.bookmarks.MoveSelectorUp();

                }
            }
        }

        else {

            verticalDown = false;
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

            if (Controller.instance.bookmarks.bookmarkPanelActivated) {

                Controller.instance.bookmarks.SelectBookmark();

            }

            //If no POI selected, then try to select a new POI
            else if (Controller.selectedPOI == null)
            {
                RaycastHit hit;
                Physics.Raycast(Controller.instance.raycastCam.transform.position, Controller.instance.raycastCam.transform.forward, out hit, Mathf.Infinity, POILayerMask, QueryTriggerInteraction.Collide);
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

        if (Input.GetAxis("Xbox Back") == 1 && !justActivatedBack)
        {
            justActivatedStart = true;
            SceneManager.LoadScene("MultiDisplayPlanet");
        }

        else if (Input.GetAxis("Xbox Back") == 0 && justActivatedBack)
        {
            justActivatedBack = false;
        }

        if (Input.GetAxis("Xbox Start") == 1 && !justActivatedStart)
        {
            justActivatedStart = true;
            Controller.instance.Toggle3D();
        }

        else if (Input.GetAxis("Xbox Start") == 0 && justActivatedStart)
        {
            justActivatedStart = false;
        }

        if (Input.GetAxis("LeftTrigger") == 1) {

            if (!Controller.instance.bookmarks.bookmarkPanelActivated) {

                Controller.instance.bookmarks.MovePanelUp();
            }

        }

        else if (Controller.instance.bookmarks.bookmarkPanelActivated) {

            Controller.instance.bookmarks.MovePanelDown();

        }

        if (Input.GetAxis("RightTrigger") == 1) {


            if (!Controller.instance.inputGuide.panelActive) {

                Controller.instance.inputGuide.MovePanelUp();

            }
        }

        else if (Controller.instance.inputGuide.panelActive) {

            Controller.instance.inputGuide.MovePanelDown();

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
