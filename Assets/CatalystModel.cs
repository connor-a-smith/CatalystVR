using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CatalystModel : CatalystSiteElement {

    public GameObject model;

    private float rotationSpeed = 0.1f;

    private bool rotate = true;

    protected override IEnumerator InitializeCoroutine(SerializableCatalystSiteElement siteData)
    {

        if (siteData is SerializableModel)
        {
            yield return StartCoroutine(LoadModel(siteData as SerializableModel));
        }
        else
        {
            PrintIncorrectTypeError(siteData.name, "Model");
        }

    }

    protected override IEnumerator ActivateCoroutine(SerializableCatalystSiteElement siteData)
    {

        if (model == null) {

            yield return Initialize(siteData);

        }

        model.SetActive(true);

    }

    protected override IEnumerator DeactivateCoroutine()
    {

        model.SetActive(false);

        yield return null;


    }

    public void SetLocalPosition(Vector3 position)
    {

        model.transform.localPosition = position;

    }

    public void Update()
    {

        if (model.activeSelf && rotate)
        {

            model.transform.Rotate(Vector3.up, rotationSpeed);

        }

    }

    public IEnumerator LoadModel(SerializableModel modelData)
    {

        string path = modelData.filePath;

        if (Path.GetExtension(path) == ".dae")
        {
            string colladaString = File.ReadAllText(path);
            model = ColladaImporter.Import(colladaString);

            if (model == null)
            {
                Debug.LogError("Failed to load model from " + path);
            }

            yield return null;
        }
        else
        {

            Debug.LogError("Failed to load model, unsupported file extension: " + path);

        }

        model.SetActive(false);
   }


}

[System.Serializable]
public class SerializableModel : SerializableCatalystSiteElement
{

    public string filePath;

}
