using UnityEngine;
using System.Collections;

/// <summary>
/// A loadable photo that will rotate around a user and can be selected for viewing.
/// </summary>
public class CatalystPhoto : MonoBehaviour {

    // Speed at which moving pictures should rotate around the player
    [SerializeField] private float rotateSpeed = 0.05f;

    // Whether or not the pictures should be spinning
    public bool isSpinning = false;

    // If this picture has been selected
    private bool selected;

    // Keeps track of where the picture should be in the circle
    private GameObject placeHolder;

    // Update is called once per frame
    private void Update() {

        // Faces the pictures toward the camera/user
        Vector3 cameraPos = Camera.main.transform.position;
        transform.LookAt(Camera.main.transform.position, Vector3.up);

        if(isSpinning) {
            // If this picture was not selected, then rotate the picture.
            if(!selected) {

                // Rotate the picture around the camera's position
                this.transform.RotateAround(Camera.main.transform.position, new Vector3(0, 1, 0), rotateSpeed);
            }

            // If the picture has been selected, the rotate the placeholder.
            else {
                if(placeHolder) {
                    placeHolder.transform.RotateAround(Camera.main.transform.position, new Vector3(0, 1, 0), rotateSpeed);
                }
            }
        }
    }

    /// <summary>
    /// Moves the photo between the player and it's correct position among other pictures.
    /// </summary>
    public void ImageTransition() {

        // If this photo was already selected, move it back to it's position
        if (selected) {
            StartCoroutine(LerpToPosition());
        }

        // If this photo has not been selected, lerp it in front of the camera.
        else {
            StartCoroutine(LerpToCam());
        }

    }

    /// <summary>
    /// Whether or not this photo has been selected.
    /// </summary>
    /// <returns> True if photo is actively selected by player. </returns>
    public bool IsSelected() {
        return selected;
    }

    /// <summary>
    /// Moves the photo in front of the player camera.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LerpToCam() {

        // Distance in front of the Camera that the photo should be.
        float camOffset = 5.0f;

        // How long the transition should take.
        float moveDuration = 1.0f;

        // The photo is now selected.
        selected = true;

        // Create a new placeholder to store where the photo should return to.
        placeHolder = new GameObject();
        placeHolder.name = "photoPlaceholder"; //give it a reasonable name
        placeHolder.transform.position = this.transform.position; // Move it to current photo position

        // Stores the start position of the movement.
        Vector3 startPosition = this.transform.position;

        // Calculates the end position of the picture for the transition.
        Vector3 endPosition = Camera.main.transform.position + (Camera.main.transform.forward * camOffset);

        // Moves the photo to the camera over a period of time.
        for (float i = 0; i < moveDuration; i+= Time.deltaTime) {

            // Updates the position of the photo.
            transform.position = Vector3.Lerp(startPosition, endPosition, i / moveDuration);

            // Wait for next frame to continue movement
            yield return null;

        }

        // Snap photo to end position in event that it didn't make it all the way.
        // This is often the case since floats can be weird, so we make sure end is exact.
        transform.position = endPosition;
    }

    /// <summary>
    /// Moves the photo from the camera back to it's correct position among other photos
    /// </summary>
    /// <returns></returns>
    private IEnumerator LerpToPosition() {

        // Stores the current position of the photo.
        Vector3 startPosition = this.transform.position;

        // How long the transition should take.
        float moveDuration = 1.0f;

        // Move the photo over a period of time.
        for (float i = 0; i < moveDuration; i+=Time.deltaTime) {
            transform.position = Vector3.Lerp(startPosition, placeHolder.transform.position, i / moveDuration);
            yield return null; //wait for next frame
        }

        transform.position = placeHolder.transform.position; //snap
        selected = false; //deselect

        // Destroy the placeholder since it's no longer needed.
        GameObject.Destroy(placeHolder);

        // Remove placeholder reference (Should already be null, but let's be safe).
        placeHolder = null;
    }
}