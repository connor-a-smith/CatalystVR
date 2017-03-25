using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveCamLoader : MonoBehaviour
{

    [SerializeField]
    private Material leftEyeCubemap;
    
    [SerializeField]
    private Material rightEyeCubemap;

    private List<Camera> leftEyeCameras;
    private List<Camera> rightEyeCameras;

    [System.Obsolete]
    private GameObject skyboxCameras;

    // Use this for initialization
    void Start()
    {

        leftEyeCameras = CAVECameraRig.leftEyeCameras;
        rightEyeCameras = CAVECameraRig.rightEyeCameras;

        CAVECameraRig.on3DToggled += Set3D;

        SetupCubemapOnCameras();

        Set3D(CAVECameraRig.is3D);

    }


    private void SetupCubemapOnCameras()
    {

        foreach (Camera cam in CAVECameraRig.allCameras)
        {

            StereoTargetEyeMask targetEye = cam.stereoTargetEye;
            Material skyboxMaterial = rightEyeCubemap;

            if (targetEye == StereoTargetEyeMask.Left)
            {
                skyboxMaterial = leftEyeCubemap;
            }

            Skybox camSkybox = cam.gameObject.AddComponent<Skybox>();
            camSkybox.material = skyboxMaterial;

        }
    }

    private void RemoveCubemapFromCameras()
    {
        foreach (Camera cam in CAVECameraRig.allCameras)
        {
            Destroy(cam.GetComponent<Skybox>());
        }
    }

    public void Set3D(bool is3D)
    {
        foreach (Camera cam in leftEyeCameras)
        {

            Skybox camSkybox = cam.GetComponent<Skybox>();

            if (is3D)
            {
                camSkybox.material = leftEyeCubemap;
            }

            else
            {
                camSkybox.material = rightEyeCubemap;
            }
        }
    }

    public void OnDestroy()
    {

        CAVECameraRig.on3DToggled -= Set3D;

        RemoveCubemapFromCameras();

    }

    [System.Obsolete]
    public void CreateSkyboxCameras()
    {
        skyboxCameras = new GameObject();
        skyboxCameras.transform.parent = leftEyeCameras[0].transform.parent;
        skyboxCameras.name = "Skybox Cameras";

        foreach (Camera camera in leftEyeCameras)
        {

            // First, add the right eye to this camera for non-3D mode.
            camera.gameObject.AddComponent<Skybox>().material = rightEyeCubemap;

            // Then, create a new skybox camera that uses the correct cubemap.
            GameObject newCamera = GameObject.Instantiate(camera.gameObject, camera.transform.position, camera.transform.rotation) as GameObject;
            newCamera.transform.parent = skyboxCameras.transform;
            newCamera.GetComponent<Camera>().cullingMask = 0;
            newCamera.GetComponent<Skybox>().material = leftEyeCubemap;
            newCamera.GetComponent<Camera>().depth = -1;
        }

        foreach (Camera camera in rightEyeCameras)
        {

            // First, add the right eye to this camera for non-3D mode.
            camera.gameObject.AddComponent<Skybox>().material = rightEyeCubemap;

            // Then, create a new skybox camera that uses the correct cubemap.
            GameObject newCamera = GameObject.Instantiate(camera.gameObject, camera.transform.position, camera.transform.rotation) as GameObject;
            newCamera.transform.parent = skyboxCameras.transform;
            newCamera.GetComponent<Camera>().cullingMask = 0;
            newCamera.GetComponent<Skybox>().material = rightEyeCubemap;
            newCamera.GetComponent<Camera>().depth = -1;

        }

        // Initially deactivate the skybox cameras.
        SetSkyboxCamerasActive(false);


    }

    [System.Obsolete]
    public void SetSkyboxCamerasActive(bool active)
    {

        // Sets the skybox cameras to their respective active state.
        skyboxCameras.SetActive(active);

        foreach (Camera camera in leftEyeCameras)
        {

            if (active)
            {
                // If the skybox cameras are active, the normal cameras shouldn't render a skybox.
                camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
            }
            else
            {
                camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            }
        }

        foreach (Camera camera in rightEyeCameras)
        {

            if (active)
            {
                // If the skybox cameras are active, the normal cameras shouldn't render a skybox.
                camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
            }
            else
            {
                camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            }
        }
    }
}
