using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GamepadInputHandler : MonoBehaviour {

    public static float timeSinceLastInput = 0.0f;

    private List<string> activeInputs;

    [SerializeField]
    private float timeBetweenSceneCycles = 10.0f;

    [SerializeField]
    private float fadeTime = 2.0f;

    [SerializeField]
    private float minutesUntilReset = 5.0f;


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

    /*
    public void CreateInputEvent(InputOption input, Action function) {




    }

    private void Awake()
    {

        gameManager = GetComponentInParent<GameManager>();

    }

    // Use this for initialization
    void Start() {

        //Set raycast to hit everything except Ignore Raycast.
        POILayerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
        StartCoroutine(CheckForInput());
    }

    // Update is called once per frame
    void Update() {


        for (int i = 0; i < inputOptions.Length; i++)
        {

            if (Input.GetAxis(inputOptions[i].))


        }

        float inputValue;

        if ((inputValue = Input.GetAxis(RIGHT_STICK_HORIZONTAL)) != 0)
        {
            onRightStickHorizontal(inputValue);
            SetInputActive(RIGHT_STICK_HORIZONTAL);
        }

        if ((inputValue = Input.GetAxis(LEFT_STICK_HORIZONTAL)) != 0)
        {
            onLeftStickHorizontal(inputValue);
            activeInputs.Add(LEFT_STICK_HORIZONTAL);
        }

        if ((inputValue = Input.GetAxis(RIGHT_STICK_VERTICAL)) != 0)
        {
            onRightStickVertical(inputValue);
            activeInputs.Add(RIGHT_STICK_HORIZONTAL);
        }

        if ((inputValue = Input.GetAxis(LEFT_STICK_VERTICAL)) != 0)
        {
            onLeftStickVertical(inputValue);
        }

        if(Input.GetAxis("RightStickHorizontal") != 0) {

            //POI selected and not advanced mode, stick controller POI movement.
            else if(!justActivatedRightStickHorizontal) {

                justActivatedRightStickHorizontal = true;

                PhotoComponent POIPhotos = GameManager.selectedPOI.GetComponent<PhotoComponent>();

                if(POIPhotos != null && POIPhotos.loaderObj != null && POIPhotos.loaderObj.activeSelf) {

                    if(Input.GetAxis("RightStickHorizontal") > 0) {

                        POIPhotos.loaderObj.GetComponent<PhotoController>().MoveRight();


                    }
                    else {

                        POIPhotos.loaderObj.GetComponent<PhotoController>().MoveLeft();
                    }
                }
                else {
                    //Debug.Log(Input.GetAxis("RightStickHorizontal"));
                    if(Input.GetAxis("RightStickHorizontal") > 0) {
                        pickActiveButton(1);
                    }
                    else {
                        pickActiveButton(-1);
                    }
                }
            }
        }

        //On release of stick, allow to be used again.
        else if(Input.GetAxis("RightStickHorizontal") == 0 && justActivatedRightStickHorizontal) {
            justActivatedRightStickHorizontal = false;
        }

        if(Input.GetAxis("RightStickVertical") != 0) {

            timeSinceLastInput = 0.0f;

            if((GameManager.selectedPOI == null && !GameManager.instance.bookmarks.bookmarkPanelActivated) || advancedMode) {

            }
            else if(GameManager.selectedPOI != null && !justActivatedRightStickHorizontal) {

                PhotoComponent POIPhotos = GameManager.selectedPOI.GetComponent<PhotoComponent>();

                if(POIPhotos != null && POIPhotos.loaderObj != null && POIPhotos.loaderObj.activeSelf) {

                    if(Input.GetAxis("RightStickVertical") > 0) {

                        POIPhotos.loaderObj.GetComponent<PhotoController>().MoveUp();


                    }
                    else {

                        POIPhotos.loaderObj.GetComponent<PhotoController>().MoveDown();
                    }
                }
            }
        }

        if(Input.GetAxis("Horizontal") != 0) {

            //POI selected and not advanced mode, stick controller POI movement.
            else if(!GameManager.instance.bookmarks.bookmarkPanelActivated && !justActivatedLeftStickHorizontal) {
                justActivatedLeftStickHorizontal = true;

                PhotoComponent POIPhotos = GameManager.selectedPOI.GetComponent<PhotoComponent>();

                if(POIPhotos != null && POIPhotos.loaderObj != null && POIPhotos.loaderObj.activeSelf) {

                    if(Input.GetAxis("Horizontal") > 0) {

                        POIPhotos.loaderObj.GetComponent<PhotoController>().MoveRight();


                    }
                    else {

                        POIPhotos.loaderObj.GetComponent<PhotoController>().MoveLeft();
                    }
                }
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

        //On release of stick, allow to be used again.
        else if(Input.GetAxis("Horizontal") == 0 && justActivatedLeftStickHorizontal) {
            justActivatedLeftStickHorizontal = false;
        }

        if(Input.GetAxis("Vertical") != 0) {

            timeSinceLastInput = 0.0f;

            else if(!justActivatedLeftStickVertical) {

                justActivatedLeftStickVertical = true;

                if(GameManager.instance.bookmarks.bookmarkPanelActivated) {

                    if(Input.GetAxis("Vertical") < 0) {

                        GameManager.instance.bookmarks.MoveSelectorDown();


                    }
                    else {

                        GameManager.instance.bookmarks.MoveSelectorUp();

                    }

                }
                else if(GameManager.selectedPOI != null) {

                    PhotoComponent POIPhotos = GameManager.selectedPOI.GetComponent<PhotoComponent>();

                    if(POIPhotos != null && POIPhotos.loaderObj != null && POIPhotos.loaderObj.activeSelf) {

                        if(Input.GetAxis("Vertical") > 0) {

                            POIPhotos.loaderObj.GetComponent<PhotoController>().MoveUp();


                        }
                        else {

                            POIPhotos.loaderObj.GetComponent<PhotoController>().MoveDown();

                        }
                    }
                }
            }
        }
        else if(Input.GetAxis("Vertical") == 0 && justActivatedLeftStickVertical) {

            justActivatedLeftStickVertical = false;

        }


        if(Input.GetAxis("Xbox DpadX") != 0 && !justActivatedDpad) {
            if(GameManager.selectedPOI != null) {
                pickActiveButton((int)Input.GetAxis("Xbox DpadX"));
                justActivatedDpad = true;
                timeSinceLastInput = 0.0f;

            }
        }

        //On release of dpad, allow to be used again.
        else if(Input.GetAxis("Xbox DpadX") == 0 && justActivatedDpad) {
            justActivatedDpad = false;
        }

        //On a press, activate
        if(Input.GetAxis("Xbox A") != 0 && !justActivatedA) {
            justActivatedA = true;
            timeSinceLastInput = 0.0f;


            if(GameManager.instance.bookmarks.bookmarkPanelActivated) {

                GameManager.instance.bookmarks.SelectBookmark();
                pickActiveButton(0);

            }

            //If no POI selected, then try to select a new POI
            else if(GameManager.selectedPOI == null) {
                RaycastHit hit;
                Physics.Raycast(GameManager.instance.raycastCam.transform.position, GameManager.instance.raycastCam.transform.forward, out hit, Mathf.Infinity, POILayerMask, QueryTriggerInteraction.Collide);
                GameManager.inputManager.HandleHit(hit);


                //If a POI was selected by this hit, we need to set the active GUI object.
                if(GameManager.selectedPOI != null) {
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

            if(GameManager.selectedPOI != null) {
                GameManager.selectedPOI.Deactivate();
            }
        }

        //On release of button, allow to be used again.
        else if(Input.GetAxis("Xbox B") == 0) {
            justActivatedB = false;
        }

        if(Input.GetAxis("Xbox Back") == 1 && !justActivatedBack) {

            timeSinceLastInput = 0.0f;

            justActivatedStart = true;
            SceneManager.LoadScene("MultiDisplayPlanet");
        }
        else if(Input.GetAxis("Xbox Back") == 0 && justActivatedBack) {
            justActivatedBack = false;
        }

        if(Input.GetAxis("Xbox Start") == 1 && !justActivatedStart) {

            timeSinceLastInput = 0.0f;

            justActivatedStart = true;

            gameManager.cameraRig.Toggle3D();
        }
        else if(Input.GetAxis("Xbox Start") == 0 && justActivatedStart) {
            justActivatedStart = false;
        }

        if(Input.GetAxis("LeftTrigger") == 1) {

            timeSinceLastInput = 0.0f;

            if(!GameManager.instance.bookmarks.bookmarkPanelActivated) {

                GameManager.instance.bookmarks.MovePanelUp();
            }

        }
        else if(GameManager.instance.bookmarks.bookmarkPanelActivated) {

            GameManager.instance.bookmarks.MovePanelDown();

        }

        if(Input.GetAxis("RightTrigger") == 1) {

            timeSinceLastInput = 0.0f;

            if(!GameManager.instance.inputGuide.panelActive) {

                GameManager.instance.inputGuide.MovePanelUp();

            }
        }
        else if(GameManager.instance.inputGuide.panelActive) {

            GameManager.instance.inputGuide.MovePanelDown();

        }

        //TODO Remove
        if(Input.GetKey(KeyCode.A)) {//else if (Input.GetButton("joystick button 0"))
            SceneManager.LoadScene("Mar Saba");
        }
        if(Input.GetKey(KeyCode.S)) {//else if (Input.GetButton("joystick button 0"))
            SceneManager.LoadScene("Luxor");
        }
        if(Input.GetKey(KeyCode.D)) {//else if (Input.GetButton("joystick button 0"))
            SceneManager.LoadScene("MultiDisplayPlanet");
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


    public IEnumerator CheckForInput() {

        while(true) {

            if(timeSinceLastInput >= 0) {

                timeSinceLastInput += Time.deltaTime;

            }

            if(timeSinceLastInput > (minutesUntilReset * 60.0f)) {

                gameManager.cameraRig.Set3D(false);

                timeSinceLastInput = -1.0f;

                StartCoroutine(CycleScenes());

                GameManager.gameState = GameManager.State.IDLE;

            }

            yield return null;

        }
    }

    public IEnumerator CycleScenes() {

        yield return StartCoroutine(FadePlane(true, fadeTime));

        SetPlatformVisible(false);

        Coroutine rotationRoutine = StartCoroutine(GameManager.instance.SetIdleRotatePlatform());

        Coroutine cyclingCoroutine = StartCoroutine(CycleSceneTransition());

        yield return StartCoroutine(WatchForControllerInput(cyclingCoroutine));

        yield return StartCoroutine(FadePlane(true, 0.01f));

        SetPlatformVisible(true);

        StopCoroutine(rotationRoutine);

        // NOTE: This line resets the user back to the Earth scene. May want to change later.
        SceneManager.LoadScene(0);

        yield return StartCoroutine(FadePlane(false, fadeTime/2));

    }

    public IEnumerator CycleSceneTransition() {

        while(true) {

            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if(sceneIndex >= SceneManager.sceneCountInBuildSettings) {

                sceneIndex = 1;

            }

            yield return StartCoroutine(FadePlane(true, fadeTime));

            SceneManager.LoadScene(sceneIndex);

            yield return StartCoroutine(FadePlane(false, fadeTime));

            yield return new WaitForSeconds(timeBetweenSceneCycles);

        }
    }

    public IEnumerator WatchForControllerInput(Coroutine coroutineToStop) {

        while(true) {

            if(timeSinceLastInput >= 0) {

                Debug.LogWarning("Stopping");
                StopCoroutine(coroutineToStop);
                GameManager.gameState = GameManager.State.ACTIVE;
                break;

            }

            yield return null;
        }
    }

    public void SetPlatformVisible(bool visible) {

        GameManager.instance.platformMonitor.SetActive(visible);
        GameManager.instance.platformModel.SetActive(visible);
        GameManager.instance.bookmarkPanel.SetActive(visible);
        GameManager.instance.controllerPanel.SetActive(visible);

    }

    public IEnumerator FadePlane(bool fadeIn, float newFadeTime) {

        GameObject plane = GameManager.instance.fadePlane;

        MeshRenderer renderer = plane.GetComponent<MeshRenderer>();

        Color startColor = renderer.material.color;
        Color endColor = startColor;


        if (fadeIn) {

            endColor.a = 1.0f;

        }
        else {

            endColor.a = 0.0f;
        }

        if (startColor.a != endColor.a) {

            for (float i = 0; i < fadeTime; i += Time.deltaTime) {

                renderer.material.SetColor("_Color", Color.Lerp(startColor, endColor, i / newFadeTime));

                yield return null;

            }

            renderer.material.SetColor("_Color", endColor);

        }
    }

    private bool IsInputActive(string inputString)
    {
        return activeInputs.Contains(inputString);
    }

    private void SetInputActive(string inputString)
    {

        if (!IsInputActive(inputString))
        {
            activeInputs.Add(inputString);
        }

    }
    */
}
