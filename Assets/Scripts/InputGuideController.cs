using UnityEngine;
using System.Collections;
using UnityEngine.UI; //to enable getComponent<Text>();

public class InputGuideController : MonoBehaviour {

	//while the L is being pressed, then keep showing
	private bool initActivated = false;

    public bool panelActive = false;

	private Animator platformAnim;

	public GameObject controlsPanel;

	// Use this for initialization
	void Start () {

		platformAnim = Controller.playerShip.GetComponent<Animator> ();

        MovePanelUp();

        panelActive = false;

	}

	/// <summary>
	/// Move the animation for going up
	/// </summary>
	public void MovePanelUp() {

        float animationStartTime = 0.0f;

        //for the initial idle animation
        if (platformAnim.GetCurrentAnimatorStateInfo(1).IsName("ControlsFally")) {

            float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo(1).normalizedTime); //never go higher than 1
            platformAnim.StopPlayback();
            animationStartTime = 1.0f - playbackTime;
        }

        if (!platformAnim.GetCurrentAnimatorStateInfo(1).IsName("ControlsFloaty")) {

            platformAnim.Play("ControlsFloaty", 1, animationStartTime);


        }

        panelActive = true;



    }

    /// <summary>
    /// Move the animation for going down
    /// </summary>
    public void MovePanelDown() {

        float animationStartTime = 0.0f;

        if (platformAnim.GetCurrentAnimatorStateInfo (1).IsName ("ControlsFloaty")) {

			float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo (1).normalizedTime); //never go higher than 1
			platformAnim.StopPlayback ();
			animationStartTime = 1.0f - playbackTime;

        }

        platformAnim.Play("ControlsFally", 1, animationStartTime);

        panelActive = false;
        initActivated = true;

	}
}
