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

    protected override IEnumerator ActivateCoroutine()
    {

        if (model != null)
        {

            model.SetActive(true);
            yield return null;

        }

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
            LoadTextures(model, colladaString);
            
            if (model == null)
            {
                Debug.LogError("Failed to load model from " + path);
            }

            yield return null;
        }
        else if (Path.GetExtension(path) == ".obj")
        {

            Debug.LogWarning("LOADING OBJ");

            Material defaultMat = new Material(Shader.Find("Standard"));
            GameObject[] objects = ObjReader.use.ConvertFile(path, true, defaultMat);

            model = new GameObject(modelData.name);
            model.transform.parent = this.transform;

            foreach (GameObject obj in objects)
            {

                obj.transform.parent = model.transform;

            }

            Debug.LogWarning("RESULT: " + objects[0]);

        }
        else
        {

            Debug.LogError("Failed to load model, unsupported file extension: " + path);
            yield break;
        }

        if (model == null)
        {
            yield break;
        }

       // model.SetActive(false);

   }

    private IEnumerator LoadTextures(GameObject go, string originalUrl)
    {
        string path = originalUrl;
        int lastSlash = path.LastIndexOf('/', path.Length - 1);
        if (lastSlash >= 0) path = path.Substring(0, lastSlash + 1);
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                if (m.mainTexture != null)
                {
                    Texture2D texture = null;
                    string texUrl = path + m.mainTexture.name;
                    yield return StartCoroutine(LoadTexture(texUrl, retval => texture = retval));
                    if (texture != null)
                    {
                        m.mainTexture = texture;
                    }
                }
            }
        }
    }

    private IEnumerator LoadTexture(string url, System.Action<Texture2D> result)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {

            Debug.LogError("Failed to load texture: " + url);

        }
        else
        {
        }
        result(www.texture);
    }


}

[System.Serializable]
public class SerializableModel : SerializableCatalystSiteElement
{

    public string filePath;

}
