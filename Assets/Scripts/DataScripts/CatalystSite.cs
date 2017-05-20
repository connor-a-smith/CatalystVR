using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class CatalystSite : MonoBehaviour
{

    public string siteName
    {
        get
        {
            return siteData.name;

        }
    }

    public List<CAVECam> caveCams;
    public List<CatalystModel> models;
    private Transform modelParent;
    private int camIndex = 0;

    [SerializeField] private float modelDistanceFromCam = 3.0f;

    public SerializableCatalystSite siteData;

    public CatalystSiteElement activeElement;

    public IEnumerator ShowCAVECams()
    {

        DeactivateActiveElement();

        if (caveCams == null || caveCams.Count == 0)
        {

            yield return StartCoroutine(LoadCAVECams());

        }

        camIndex = 0;

        SceneManager.LoadSceneAsync("Buffer Scene");

        if (camIndex < caveCams.Count)
        {

            caveCams[camIndex].Activate();
            activeElement = caveCams[camIndex];

        }
        else
        {
            Debug.LogError("No panos loaded. Aborting.");
        }

    }

    public IEnumerator HideCAVECams()
    {

        DeactivateActiveElement();
        yield return null;

    }

    public IEnumerator ShowModels()
    {

        DeactivateActiveElement();

        if (models == null || models.Count == 0)
        {

            yield return StartCoroutine(LoadModels());

        }

        SceneManager.LoadSceneAsync("Buffer Scene");

        modelParent.localPosition = Vector3.zero;

        List<Vector3> positions = CommonCatalystMath.GetPositionsOnUnitCircleBySides(models.Count);

        for (int i = 0; i < positions.Count; i++)
        {

            positions[i] *= modelDistanceFromCam;

        }

        for (int i = 0; i < models.Count; i++)
        {

            models[i].Activate();
            models[i].SetLocalPosition(positions[i]);
            activeElement = models[i];

        }
    }

    public IEnumerator HideModels()
    {

        foreach (CatalystModel model in models)
        {

            yield return model.Deactivate();

        }
    }

    public void DeactivateActiveElement()
    {

        if (activeElement != null)
        {
            activeElement.Deactivate();
        }

    }

    public void CycleCAVECams()
    {
        if (activeElement is CAVECam)
        {

            activeElement.Deactivate();

            camIndex++;
            if (camIndex > caveCams.Count)
            {
                camIndex = 0;
            }

            caveCams[camIndex].Activate();
            activeElement = caveCams[camIndex];

        }
        else
        {

            Debug.LogWarning("Unable to cycle CAVECams: No CAVECam set is currently active. Please call ShowCAVECams first");

        }
    }

    public IEnumerator LoadCAVECams()
    {

        if (caveCams == null || caveCams.Count == 0)
        {

            caveCams = new List<CAVECam>();

            for (int i = 0; i < siteData.panos.Length; i++)
            {

                SerializableCAVECam camJSON = siteData.panos[i];

                Debug.LogFormat("Initializing pano {0} of {1}", i+1, siteData.panos.Length);

                CAVECam newCam = gameObject.AddComponent<CAVECam>();
                yield return newCam.Initialize(camJSON);

                caveCams.Add(newCam);

            }
        }
    }

    public IEnumerator LoadModels()
    {

        models = new List<CatalystModel>();

        modelParent = new GameObject().transform;

        modelParent.name = siteData.name + " - Images";

        modelParent.parent = GameManager.instance.cameraRig.viewpoint.transform;

        modelParent.transform.localPosition = Vector3.zero;
        modelParent.transform.localRotation = Quaternion.identity;

        foreach (SerializableModel modelData in siteData.models)
        {

            CatalystModel newModel = new CatalystModel();
            yield return newModel.Initialize(modelData);
            newModel.transform.parent = modelParent;

        }
    }
}


[System.Serializable]
public class SerializableCatalystSite
{

    public string name;
    public string description;
    public float latitude;
    public float longitude;

    public SerializableCAVECam[] panos;
    public SerializableVideo[] videos;
    public SerializableModel[] models;
    public SerializableImage[] images;
    public SerializablePointCloud[] pointClouds;

}