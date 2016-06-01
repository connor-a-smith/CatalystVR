using UnityEngine;
using System.Collections;

public class CatalystPhoto : MonoBehaviour {

    private float rotateSpeed = 0.05f;
    private bool selected;
    GameObject placeHolder;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 cameraPos = Camera.main.transform.position;
        transform.LookAt(Camera.main.transform.position, Vector3.up);

        // transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), Space.Self);
        if(!selected) {
            this.transform.RotateAround(Camera.main.transform.position, new Vector3(0, 1, 0), rotateSpeed);
        }
        else {
            if (placeHolder) {
                placeHolder.transform.RotateAround(Camera.main.transform.position, new Vector3(0, 1, 0), rotateSpeed);
            }
        }
    }

    private void SelectImage() {

        StartCoroutine(LerpToCam());
    }

    private void DeselectImage() {

        StartCoroutine(LerpToPosition());

    }


    public void ImageTransition() {

        if (selected) {
            DeselectImage();
        }
        else {
            SelectImage();
        }

    }

    public bool IsSelected() {

        return selected;

    }


    IEnumerator LerpToCam() {

        float camOffset = 5.0f;
        float moveDuration = 1.0f;

        selected = true;
        placeHolder = new GameObject();
        placeHolder.transform.position = this.transform.position;

        Vector3 startPosition = this.transform.position;
        Vector3 endPosition = Camera.main.transform.position + (Camera.main.transform.forward * camOffset);

        for (float i = 0; i < moveDuration; i+= Time.deltaTime) {

            transform.position = Vector3.Lerp(startPosition, endPosition, i / moveDuration);
            yield return null;

        }

        transform.position = endPosition;

    }

    IEnumerator LerpToPosition() {

        Vector3 startPosition = this.transform.position;
        float moveDuration = 1.0f;

        for (float i = 0; i < moveDuration; i+=Time.deltaTime) {

            transform.position = Vector3.Lerp(startPosition, placeHolder.transform.position, i / moveDuration);
            yield return null;
        }

        transform.position = placeHolder.transform.position;
        selected = false;
        GameObject.Destroy(placeHolder);
        placeHolder = null;
    }
}
