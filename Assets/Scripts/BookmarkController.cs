using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	public bool noSelection;

	public List<Bookmark> bookmarks;

	private int bookmarkIndex;

    void Awake() {

        bookmarks = new List<Bookmark>();


    }

    // Use this for initialization
    void Start () {

		bookmarkIndex = 0;

		platformAnim = GameManager.playerShip.GetComponent<Animator> ();
		initActivate = true;
		initDeactivate = false;
	}

	/// <summary>
	/// Move the animation for going up
	/// </summary>
	public void MovePanelUp() {
        //for the initial idle animation

        ChangeColor(0, selectionColor);

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

		RevertColor (bookmarkIndex);
        platformAnim.StopPlayback();

        if (platformAnim.GetCurrentAnimatorStateInfo (0).IsName ("BookmarkFloaty")) {

			float playbackTime = Mathf.Min(1.0f, platformAnim.GetCurrentAnimatorStateInfo (0).normalizedTime); //never go higher than 1
			float backwardsTime = 1.0f - playbackTime;
			platformAnim.Play ("BookmarkFally", 0, backwardsTime);

		}

		bookmarkIndex = 0; //reset the selection to the first index

        bookmarkPanelActivated = false;

    }

    void RevertColor(int index) {

        Color newColor = Color.white;
        newColor.a = 0.5f;
        ChangeColor(index, newColor);

	}

	void ChangeColor(int index, Color color) {

        Image panelImage = bookmarks[index].GetComponent<Image>();
        panelImage.color = color;

	}

	void Jump(int index) {

        Bookmark loc = bookmarks [index].GetComponent<Bookmark> ();

        loc.focusComponent.Activate();

        if (POIManager.selectedPOI != null) {

            POIManager.selectedPOI.Deactivate();

        }

        POIManager.selectedPOI = null;

        loc.POI.Activate();

	}

    public void MoveSelectorUp() {

        RevertColor(bookmarkIndex);

        bookmarkIndex--;

        if (bookmarkIndex < 0) {

            bookmarkIndex = bookmarks.Count - 1;

        }

        ChangeColor(bookmarkIndex, selectionColor);

    }

    public void MoveSelectorDown() {

        RevertColor(bookmarkIndex);

        bookmarkIndex++;

        if (bookmarkIndex > bookmarks.Count - 1) {

            bookmarkIndex = 0;

        }
        
        ChangeColor(bookmarkIndex, selectionColor);
    }

    public void SelectBookmark() {
    
        Jump( bookmarkIndex);

   }

    public void ClearBookmarks() {

        bookmarkIndex = 0;
       
        foreach (Bookmark bookmark in bookmarks) {

            GameObject.Destroy(bookmark.transform.parent.gameObject);

        }

        bookmarks.Clear();


    }
}
