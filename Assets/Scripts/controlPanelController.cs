using UnityEngine;
using System.Collections;
using UnityEngine.UI; //to enable getComponent<Text>();

public class controlPanelController : MonoBehaviour {

	//update: continuously check when the L button is pressed
	//while the L is being pressed, then keep showing
	private bool initActivate;

	private Animator platformAnim;

	public GameObject controlsPanel;
	public GameObject platform;

	// Use this for initialization
	void Start () {
		platformAnim = platform.GetComponent<Animator> ();
		initActivate = true;
		//controlsPanel.SetActive (true);
	}


	/// <summary>
	/// Move the animation for going up
	/// </summary>
	void Activate() {

		//controlsPanel.SetActive (true);


		//for the initial idle animation
		if (platformAnim.GetCurrentAnimatorStateInfo (1).IsName ("ControlsFally")) {
			
			float playbackTime = Mathf.Min (1.0f, platformAnim.GetCurrentAnimatorStateInfo (1).normalizedTime); //never go higher than 1
			platformAnim.StopPlayback ();
			float backwardsTime = 1.0f - playbackTime;
			platformAnim.Play ("ControlsFloaty", 1, backwardsTime);
		} else {
			platformAnim.StopPlayback ();
		}


	}
	/// <summary>
	/// Move the animation for going down
	/// </summary>
	void Deactivate() {

		//controlsPanel.SetActive (false);

		//if this was the initial activate, deactivate it and start regular routine
		if (initActivate) {
			/*
			platformAnim.StopPlayback ();
			platformAnim.Play ("ControlsFally", 1);
			Debug.Log ("playing controlsfally\n");
			platformAnim.Play ("ControlsFally");
			*/

			float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo (1).normalizedTime); //never go higher than 1
			platformAnim.StopPlayback ();
			float backwardsTime = 1.0f - playbackTime;
			platformAnim.Play ("ControlsFally", 1, backwardsTime);
			initActivate = false;
			Debug.LogWarning ("Test1");

		} else if (platformAnim.GetCurrentAnimatorStateInfo (1).IsName ("ControlsFloaty")) {
			float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo (1).normalizedTime); //never go higher than 1
			platformAnim.StopPlayback ();
			float backwardsTime = 1.0f - playbackTime;
			platformAnim.Play ("ControlsFally", 1, backwardsTime);
			Debug.LogWarning ("Test2");
		} //else {
			//platformAnim.StopPlayback ();
		//	Debug.LogWarning ("Test3");
		//}

	}



	// Update is called once per frame
	void Update () {
		if (initActivate) {
			if (Input.GetKey(KeyCode.C)) {
				Debug.Log ("Initial Key C");
				Deactivate ();
			}
		} else {

			if (Input.GetKey (KeyCode.C)) {
				Debug.Log ("C Pressed is true\n");
				Activate ();
			} else if (!Input.GetKey(KeyCode.C)) {
				Debug.Log ("Deactivating");
				Deactivate ();
			}
		}


	}
}
