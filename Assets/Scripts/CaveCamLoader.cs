using UnityEngine;
using System.Collections;

public class CaveCamLoader : MonoBehaviour {

  [SerializeField] private Material leftEyeCubemap;
  [SerializeField] private Material rightEyeCubemap;
  private GameObject[] leftEyeCameras;
  private GameObject[] rightEyeCameras;
  private GameObject skyboxCameras;

  // Use this for initialization
  void Start() {

    leftEyeCameras = Controller.instance.leftEyeCameras;
    rightEyeCameras = Controller.instance.rightEyeCameras;

    Controller.instance.Toggle3DDelegate += Toggle3D;

    CreateSkyboxCameras();

    Toggle3D();

  }

  public void CreateSkyboxCameras() {

    skyboxCameras = new GameObject();
    skyboxCameras.transform.parent = leftEyeCameras[0].transform.parent;
    skyboxCameras.name = "Skybox Cameras";

    foreach (GameObject camera in leftEyeCameras) {

      // First, add the right eye to this camera for non-3D mode.
      camera.AddComponent<Skybox>().material = rightEyeCubemap;

      // Then, create a new skybox camera that uses the correct cubemap.
      GameObject newCamera = GameObject.Instantiate(camera, camera.transform.position, camera.transform.rotation) as GameObject;
      newCamera.transform.parent = skyboxCameras.transform;
      newCamera.GetComponent<Camera>().cullingMask = 0;
      newCamera.GetComponent<Skybox>().material = leftEyeCubemap;

    }

    foreach (GameObject camera in rightEyeCameras) {

      // First, add the right eye to this camera for non-3D mode.
      camera.AddComponent<Skybox>().material = rightEyeCubemap;

      // Then, create a new skybox camera that uses the correct cubemap.
      GameObject newCamera = GameObject.Instantiate(camera, camera.transform.position, camera.transform.rotation) as GameObject;
      newCamera.transform.parent = skyboxCameras.transform;
      newCamera.GetComponent<Camera>().cullingMask = 0;
      newCamera.GetComponent<Skybox>().material = rightEyeCubemap;

    }

    // Initially deactivate the skybox cameras.
    SetSkyboxCamerasActive(false);

  }

  /// <summary>
  /// Sets the skybox cameras active and disables the other cameras.
  /// </summary>
  /// <param name="active">If set to <c>true</c> active.</param>
  public void SetSkyboxCamerasActive(bool active) {

    // Sets the skybox cameras to their respective active state.
    skyboxCameras.SetActive(active);

    foreach (GameObject camera in leftEyeCameras) {
     
      if (active) {
        // If the skybox cameras are active, the normal cameras shouldn't render a skybox.
        camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
      }
      else {
        camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
      }
    }

    foreach (GameObject camera in rightEyeCameras) {

      if (active) {
        // If the skybox cameras are active, the normal cameras shouldn't render a skybox.
        camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
      }
      else {
        camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
      }
    }
  }

  public void Toggle3D() {

    if (Controller.instance.is3D) {

      SetSkyboxCamerasActive(true);
     
    }
    else {

      SetSkyboxCamerasActive(false);
    }
  }

  public void OnDestroy() {

    Controller.instance.Toggle3DDelegate -= Toggle3D;

    foreach (GameObject camera in leftEyeCameras) {

      Destroy(camera.GetComponent<Skybox>());

    }

    foreach (GameObject camera in rightEyeCameras) {

      Destroy(camera.GetComponent<Skybox>());

    }

    SetSkyboxCamerasActive(false);

    Destroy(skyboxCameras);

  }
}
