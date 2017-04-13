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

        


        yield return null;

    }

    private Texture2D LoadImageAsTexture(string imagePath)
    {

        WWW w = new WWW(imagePath);

        Texture2D loadedImage = new Texture2D(1, 1);

        w.LoadImageIntoTexture(loadedImage);

        return loadedImage;

    }

    private Cubemap CreateCubemapFromCaveCamEye(SerializableCAVECamEye camEye)
    {

        Cubemap 




    }

    [System.Serializable]
    private class SerializableCAVECam
    {

        SerializableCAVECamEye leftEye;
        SerializableCAVECamEye rightEye;
        string description;

    }

    [System.Serializable]
    private class SerializableCAVECamEye
    {

        string leftFace;
        string rightFace;
        string topFace;
        string bottomFace;
        string frontFace;
        string backFace;

    }
}
