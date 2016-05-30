using UnityEngine;
using System.Collections;

public class InputSelection : MonoBehaviour {

    CatalystPhoto selectedPhoto;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        Debug.DrawRay(raycast.origin, raycast.direction *1000);

        if (Input.GetMouseButtonDown(0)) {

            Debug.Log("DOWN");

            if(Physics.Raycast(raycast, out hitInfo, Mathf.Infinity)) {

                Debug.Log("HIT");

                CatalystPhoto hitPhoto = hitInfo.collider.gameObject.GetComponent<CatalystPhoto>();

                if(hitPhoto) {

                    //if a photo has already been selected and is on screen
                    if(selectedPhoto) {

                        Debug.Log("MOVING AWAY");
                        //transition the selected photo away
                        selectedPhoto.ImageTransition();
                    }

                    //if the user clicked the same photo they had selected before
                    if(hitPhoto == selectedPhoto) {

                        //deselects the photo
                        selectedPhoto = null;

                        Debug.Log("DESELECTING");

                    }

                    //if the user clicked a different photo
                    else {

                        Debug.Log("SELECTING");
                        //sets the selected photo to the newly hit one
                        selectedPhoto = hitPhoto;
                        selectedPhoto.ImageTransition();
                    }
                }
            }
        }
	}
}
