using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAVECameraRig : MonoBehaviour {

    // Static bool to keep track of whether or not we're in 3D mode.
    public static bool is3D = false;

    // Lists of left and right eye cameras.
    public static List<Camera> leftEyeCameras;
    public static List<Camera> rightEyeCameras;
    public static List<Camera> allCameras;

    // Offset between the cameras (total offset, not halfway).
    [SerializeField]
    private float cameraEyeOffset = 0.6f;

    // Delegate that is called whenever 3D mode is toggled.
    public delegate void Toggle3DDelegate(bool is3D);
    public static Toggle3DDelegate on3DToggled;

    // Called before first frame. Prepares the displays and loads cameras.
    private void Awake()
    {
        FindCameras();
        ActivateDisplays();
 
    }

    // Ensures the cameras are in the right positions.
    void Start() {

        ResetCameraPositions();

    }

    // Toggles between 3D and 2D mode, moves cameras appropriately.
    public void Toggle3D()
    {
        if (is3D)
        {
            Make2D();
        }

        else
        {
            Make3D();
        }

        // If there are any methods in the delegate, call the delegate for 3D transition.
        if (on3DToggled != null)
        {
            on3DToggled(is3D);
        }
    }

    public void Set3D(bool shouldBe3D)
    {

        if (shouldBe3D != is3D)
        {

            Toggle3D();

        }

    }

    // Activates each display currently hooked up to the computer, to ensure we're displaying across all screens.
    private void ActivateDisplays()
    {
        foreach(Display display in Display.displays)
        {
            display.Activate();
        }
    }

    // Shifts the cameras back to 2D mode, by setting both left and right cameras at the origin.
    private void Make2D()
    {
        ShiftCamerasByXOffset(leftEyeCameras, cameraEyeOffset / 2);
        ShiftCamerasByXOffset(rightEyeCameras, -cameraEyeOffset / 2);
        is3D = false;
    }

    // Shifts the cameras to 3D mode, by offsetting left and right cameras.
    private void Make3D()
    {
        ShiftCamerasByXOffset(leftEyeCameras, -cameraEyeOffset / 2);
        ShiftCamerasByXOffset(rightEyeCameras, cameraEyeOffset / 2);
        is3D = true;
    }

    // Moves the cameras back to where they should be for 2D/3D modes.
    private void ResetCameraPositions()
    {
        if (!is3D)
        {
            MoveCamerasToXPosition(leftEyeCameras, 0.0f);
            MoveCamerasToXPosition(rightEyeCameras, 0.0f);
        }
        else
        {
            MoveCamerasToXPosition(leftEyeCameras, -cameraEyeOffset / 2);
            MoveCamerasToXPosition(rightEyeCameras, cameraEyeOffset / 2);
        }
    }

    // Shifts the cameras by a specific x offset.
    private void ShiftCamerasByXOffset(List<Camera> cameras, float offset)
    {
        foreach (Camera camera in cameras)
        {
            Vector3 newPosition = camera.transform.localPosition;
            newPosition.x += offset;
            camera.transform.localPosition = newPosition;
        }
    }

    // Moves the cameras to a specific x position.
    private void MoveCamerasToXPosition(List<Camera> cameras, float xVal)
    {
        foreach (Camera camera in cameras)
        {
            Vector3 newPosition = camera.transform.localPosition;
            newPosition.x = xVal;
            camera.transform.localPosition = newPosition;
        }
    }

    // Finds the Camera components in children, and fills the camera lists respectively.
    private void FindCameras()
    {

        foreach (Camera cam in GetComponentsInChildren<Camera>())
        {
            if (cam.stereoTargetEye == StereoTargetEyeMask.Left)
            {
                leftEyeCameras.Add(cam);
            }
            else if (cam.stereoTargetEye == StereoTargetEyeMask.Right)
            {
                rightEyeCameras.Add(cam);
            }
        }

        allCameras = new List<Camera>(leftEyeCameras.Count + rightEyeCameras.Count);
        allCameras.AddRange(leftEyeCameras);
        allCameras.AddRange(rightEyeCameras);
    }
}
