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

    CatalystSiteElement activeElement;

    public IEnumerator ShowCAVECams()
    {

        if (caveCams == null || caveCams.Count == 0)
        {

            yield return StartCoroutine(LoadCAVECams());

        }

        camIndex = 0;
        caveCams[camIndex].Activate(siteData.caveCams[camIndex]);

    }

    public IEnumerator HideCAVECams()
    {

        yield return caveCams[camIndex].Deactivate();

    }

    public IEnumerator ShowModels()
    {

        if (models == null || models.Count == 0)
        {

            yield return StartCoroutine(LoadModels());

        }
        modelParent.localPosition = Vector3.zero;

        List<Vector3> positions = CommonCatalystMath.GetPositionsOnUnitCircleBySides(models.Count);

        for (int i = 0; i < positions.Count; i++)
        {

            positions[i] *= modelDistanceFromCam;

        }

        for (int i = 0; i < models.Count; i++)
        {

            models[i].Activate(siteData.models[i]);
            models[i].SetLocalPosition(positions[i]);

        }
    }

    public IEnumerator HideModels()
    {

        foreach (CatalystModel model in models)
        {

            yield return model.Deactivate();

        }
    }

    public void CycleCAVECams()
    {
        if (activeElement is CAVECam)
        {
            camIndex++;
            if (camIndex > caveCams.Count)
            {
                camIndex = 0;
            }
        }
        else
        {

            Debug.LogWarning("Unable to cycle CAVECams: No CAVECam set is currently active. Please call ShowCAVECams first");

        }
    }

    public IEnumerator LoadCAVECams()
    {

        caveCams = new List<CAVECam>();

        foreach (SerializableCAVECam camJSON in siteData.caveCams)
        {

            CAVECam newCam = new CAVECam();
            yield return newCam.Activate(camJSON);
            caveCams.Add(newCam);

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

    public SerializableCAVECam[] caveCams;
    public SerializableVideo[] videos;
    public SerializableModel[] models;
    public SerializableImage[] images;
    public SerializablePointCloud[] pointClouds;

}