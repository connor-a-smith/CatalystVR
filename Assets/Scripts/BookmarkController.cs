using UnityEngine;
using System.Collections;
using UnityEngine.UI; //to enable getComponent<Text>();

public class BookmarkController : MonoBehaviour {

    [SerializeField] Color selectionColor;

	//update: continuously check when the L button is pressed
	//while the L is being pressed, then keep showing
	public bool bookmarkPanelActivated = false;
	private bool initActivate;
	private bool initDeactivate;

	private Animator platformAnim;

	//public GameObject bookmark;
	public GameObject bookmarkImagePanel;
	public GameObject platform;
	public bool noSelection;

	private GameObject[] locations;
	private JumpIcon[] locationPanels;

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

		locationPanels = bookmarkImagePanel.GetComponentsInChildren<JumpIcon> ();

	}

	/// <summary>
	/// Move the animation for going up
	/// </summary>
	public void MovePanelUp() {
        //for the initial idle animation

        float animationStartTime = 0.0f;
	
        if (platformAnim.GetCurrentAnimatorStateInfo(0).IsName("BookmarkFally")) {

			float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo (0).normalizedTime); //never go higher than 1
			platformAnim.StopPlayback ();
			animationStartTime = 1.0f - playbackTime;
		}

       platformAnim.Play("BookmarkFloaty", 0, animationStartTime);

        bookmarkPanelActivated = true;

    }

    /// <summary>
    /// Move the animation for going down
    /// </summary>
    public void MovePanelDown() {

		RevertColor (locationSelector);
        platformAnim.StopPlayback();

        if (platformAnim.GetCurrentAnimatorStateInfo (0).IsName ("BookmarkFloaty")) {

			float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo (0).normalizedTime); //never go higher than 1
			float backwardsTime = 1.0f - playbackTime;
			platformAnim.Play ("BookmarkFally", 0, backwardsTime);

		}

		locationSelector = 0; //reset the selection to the first index

        bookmarkPanelActivated = false;

    }

    void RevertColor(int index) {

        Color newColor = Color.white;
        newColor.a = 0.5f;
        ChangeColor(index, newColor);

	}

	void ChangeColor(int index, Color color) {

        Image panelImage = locationPanels[index].GetComponent<Image>();
        panelImage.color = color;

	}

	void Jump(int index) {

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
    void Update() {

    }

    public void MoveSelectorUp() {

        RevertColor(locationSelector);

        locationSelector--;

        if (locationSelector < 0) {

            locationSelector = locations.Length - 1;

        }

        ChangeColor(locationSelector, selectionColor);

    }

    public void MoveSelectorDown() {

        RevertColor(locationSelector);

        locationSelector++;

        if (locationSelector > locations.Length - 1) {

            locationSelector = 0;

        }
        
        ChangeColor(locationSelector, selectionColor);
    }

    public void SelectBookmark() {
    
        Jump( locationSelector);

   }
}
