using UnityEngine;
using System.Collections;
using UnityEngine.UI; //to enable getComponent<Text>();

public class JumpLocation : MonoBehaviour {

	//update: continuously check when the L button is pressed
	//while the L is being pressed, then keep showing
	private bool bookmarkPanelActivated = false;
	private bool initActivate;
	private bool initDeactivate;

	private Animator platformAnim;

	public GameObject bookmark;
	public GameObject bookmarkImagePanel;
	public GameObject platform;
	public bool noSelection;

	private GameObject[] locations;
	private Image[] locationPanels;

	private int locationSelector;

	// Use this for initialization
	void Start () {
		locationSelector = 0;
		createArray ();
		platformAnim = platform.GetComponent<Animator> ();
		initActivate = true;
		initDeactivate = false;
	}

	void createArray() {
		locationPanels = bookmarkImagePanel.GetComponentsInChildren<Image> ();
	}

	/// <summary>
	/// Move the animation for going up
	/// </summary>
	void Activate() {
		//for the initial idle animation
		if (initActivate) {
			initActivate = false;
			platformAnim.Play ("BookmarkFloaty");
		} else { //loop from the current playback time so that it continues where BookmarkFally left off
			float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo (0).normalizedTime); //never go higher than 1
			platformAnim.StopPlayback ();
			float backwardsTime = 1.0f - playbackTime;
			platformAnim.Play ("BookmarkFloaty", 0, backwardsTime);
		}

	}
	/// <summary>
	/// Move the animation for going down
	/// </summary>
	void Deactivate() {
		revertColor (locationSelector);

		if (platformAnim.GetCurrentAnimatorStateInfo (0).IsName ("BookmarkFloaty")) {
			float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo (0).normalizedTime); //never go higher than 1
			platformAnim.StopPlayback ();
			float backwardsTime = 1.0f - playbackTime;
			platformAnim.Play ("BookmarkFally", 0, backwardsTime);
		} else {
			platformAnim.StopPlayback ();
		}
			
		locationSelector = 0; //reset the selection to the first index
	}

	void revertColor(int index) {
		locationPanels [index].color = Color.white;
		Color color = locationPanels [index].color;
		color.a = .50f;
		locationPanels [index].color = color;
	}

	void changeColor(int index) {
		locationPanels [index].color = Color.blue;
		Color color = locationPanels [index].color;
		color.a = .50f;
		locationPanels [index].color = color;
	}

	void jump(int index) {
		//jump!!
		Debug.Log("JUMP!\n");
		JumpIcon loc = locationPanels [index].GetComponent<JumpIcon> ();
		GameObject jumpLocation = loc.icon;


		//Teleport to location. Shifting down by cam height so that camera is in the correct position.
		platform.transform.position = jumpLocation.transform.position - new Vector3(0, Camera.main.gameObject.transform.localPosition.y, 0);
		Vector3 angles = jumpLocation.transform.rotation.eulerAngles;
		angles.x = 0;
		angles.z = 0;
		platform.transform.rotation = Quaternion.Euler (angles);
	}
		

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.L) && !bookmarkPanelActivated) {
			Debug.Log ("L Pressed is true\n");
			bookmarkPanelActivated = true;
			Activate ();
		} else if (Input.GetKey(KeyCode.L) && bookmarkPanelActivated) {
			Debug.Log("inputted for selection\n");
			if (!noSelection) {
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
					revertColor (locationSelector);
					if (locationSelector == 0) {
						locationSelector = 1;
					} else if (locationSelector == 1) {
						locationSelector = locationPanels.Length - 1; //skip the big main panel
					} else {
						locationSelector--;
					}
					changeColor (locationSelector);
				} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
					revertColor (locationSelector);
					if (locationSelector == (locationPanels.Length - 1)) { //skip the big main panel
						locationSelector = 1;
					} else {
						locationSelector++;
					}
					changeColor (locationSelector);
				} else if (Input.GetKeyDown (KeyCode.Space)) {
					Debug.Log ("keycode space\n");
					if (locationSelector != 0) {
						jump (locationSelector);
					}
				}
			}
		} else if (!Input.GetKey(KeyCode.L) && bookmarkPanelActivated) {

			Debug.Log ("Deactivating");

			Deactivate ();
			bookmarkPanelActivated = false;

		}

	}
}
