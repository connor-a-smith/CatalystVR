using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CAVECam : MonoBehaviour
{

    private string leftEyePath;
    private string rightEyePath;

    private Material leftEye;
    private Material rightEye;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private IEnumerator LoadCamFromFile(string camPath)
    {

        string camJson = File.ReadAllText(camPath);

        SerializableCAVECam camFile = JsonUtility.FromJson<SerializableCAVECam>(camJson);


        string leftEyePath = camFile.leftEye.filePath;
        string rightEyePath = camFile.rightEye.filePath;

        Texture2D leftTexture = LoadImageAsTexture(leftEyePath);
        Texture2D rightTexture = LoadImageAsTexture(rightEyePath);

        Cubemap leftCubemap = CreateCubemapFromTexture(leftTexture);
        Cubemap rightCubemap = CreateCubemapFromTexture(rightTexture);

        Material leftMaterial = new Material(Shader.Find("Skybox/Cubemap"));
        Material rightMaterial = new Material(Shader.Find("Skybox/Cubemap"));

        leftMaterial.mainTexture = leftCubemap;
        rightMaterial.mainTexture = rightCubemap;

        leftEye = leftMaterial;
        rightEye = rightMaterial;


        yield return null;

    }

    private Texture2D LoadImageAsTexture(string imagePath)
    {

        WWW w = new WWW(imagePath);

        Texture2D loadedImage = new Texture2D(1, 1);

        w.LoadImageIntoTexture(loadedImage);

        return loadedImage;

    }

    
    // TODO: implement this
    private Cubemap CreateCubemapFromTexture(Texture2D tex)
    {

        // Garbage
        return new Cubemap(1, TextureFormat.Alpha8, false);



    }



    [System.Serializable]
    private class SerializableCAVECam
    {

        public SerializableCAVECamEye leftEye;
        public SerializableCAVECamEye rightEye;
        public string description;

    }

    [System.Serializable]
    private class SerializableCAVECamEye
    {

        public string filePath;

    }
}
