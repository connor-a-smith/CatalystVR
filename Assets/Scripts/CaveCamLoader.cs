using UnityEngine;
using System.Collections;

public class CaveCamLoader : MonoBehaviour {

  [SerializeField] private Material leftEyeCubemap;
  [SerializeField] private Material rightEyeCubemap;
  private GameObject[] leftEyeCameras;
  private GameObject[] rightEyeCameras;

  // Use this for initialization
  void Start() {

    leftEyeCameras = Controller.instance.leftEyeCameras;
    rightEyeCameras = Controller.instance.rightEyeCameras;

    Controller.instance.platformMonitor.SetActive(false);
    Controller.instance.platformModel.SetActive(false);
    Controller.instance.bookmarks.gameObject.SetActive(false);
	
    Controller.instance.Toggle3DDelegate += Toggle3D;

    Toggle3D();

  }

  public void Toggle3D() {

    Debug.LogWarning("MAKING 3D");

    if (Controller.instance.is3D) {

      foreach (GameObject camera in leftEyeCameras) {

        Skybox newSkybox = camera.GetComponent<Skybox>();

        if (newSkybox == null) {

          newSkybox = camera.AddComponent<Skybox>();

        }

        newSkybox.material = leftEyeCubemap;


      }

      foreach (GameObject camera in rightEyeCameras) {

        Skybox newSkybox = camera.GetComponent<Skybox>();

        if (newSkybox == null) {

          newSkybox = camera.AddComponent<Skybox>();

        }

        newSkybox.material = rightEyeCubemap;

      }
    }
    else {

      Debug.LogWarning("MAKING 2D");


      foreach (GameObject camera in leftEyeCameras) {

        Skybox newSkybox = camera.GetComponent<Skybox>();

        if (newSkybox == null) {

          newSkybox = camera.AddComponent<Skybox>();

        }

        newSkybox.material = rightEyeCubemap;

      }

      foreach (GameObject camera in rightEyeCameras) {

        Skybox newSkybox = camera.GetComponent<Skybox>();

        if (newSkybox == null) {

          newSkybox = camera.AddComponent<Skybox>();

        }

        newSkybox.material = rightEyeCubemap;

      }
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


    Controller.instance.platformMonitor.SetActive(true);
    Controller.instance.platformModel.SetActive(true);
    Controller.instance.bookmarks.transform.parent.gameObject.SetActive(true);

  }
}
