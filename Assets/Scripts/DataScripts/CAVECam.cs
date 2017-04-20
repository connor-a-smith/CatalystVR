using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Drawing;

public class CAVECam : MonoBehaviour
{

    public const string cacheLocation = GameManager.cacheDirectory + "/CAVECams";

    public const int FRONT_INDEX = 0;
    public const int BACK_INDEX = 1;
    public const int UP_INDEX = 2;
    public const int DOWN_INDEX = 3;
    public const int RIGHT_INDEX = 4;
    public const int LEFT_INDEX = 5;

    private string leftEyePath;
    private string rightEyePath;

    public Material leftEye;
    public Material rightEye;

    private string cacheDirectory;

    [SerializeField] private string defaultCamPath = "./defaultCam.json";

    [SerializeField] private string defaultLeftEyePath = "./leftEye.tif";
    [SerializeField] private string defaultRightEyePath = "./rightEye.tif";


    // Use this for initialization
    void Start()
    {
        Debug.Log("Starting");

        StartCoroutine(LoadCamFromFile("garbage"));

    }

    private IEnumerator LoadCamFromFile(string camJSONPath)
    {

        SerializableCAVECam camFile;

        if (File.Exists(camJSONPath))
        {

            string camJson = File.ReadAllText(camJSONPath);

            camFile = JsonUtility.FromJson<SerializableCAVECam>(camJson);

            Debug.Log("Found Cam. Loading");

            yield return null;

        }

        else
        {

            if (File.Exists(defaultCamPath))
            {

                Debug.LogWarning("No CAVECam JSON found at " + camJSONPath + ". Attempting to use default path: " + defaultCamPath);

                string objJSON = File.ReadAllText(defaultCamPath);

                camFile = JsonUtility.FromJson<SerializableCAVECam>(objJSON);

                yield return null;

            }
            else
            {

                if (!File.Exists(defaultLeftEyePath) || !File.Exists(defaultRightEyePath))
                {
                    Debug.LogErrorFormat("Cannot load CAVECam, and no defaults set. Please create defaults to prevent this error");
                    yield break;
                }

                camFile = new SerializableCAVECam(defaultLeftEyePath, defaultRightEyePath, "Default Text");

                File.WriteAllText(defaultCamPath, JsonUtility.ToJson(camFile));

                Debug.LogWarningFormat("No CAVECam JSON found at {0}, and no default JSON at {1}. Creating a blank cam with left eye {2} and right eye {3}.", camJSONPath, defaultCamPath, defaultLeftEyePath, defaultRightEyePath);

                yield return null;

            }
        }


        leftEyePath = camFile.leftEyePath;
        rightEyePath = camFile.rightEyePath;

        List<Texture2D> leftTextures = new List<Texture2D>();
        List<Texture2D> rightTextures = new List<Texture2D>();

        if (Directory.Exists(GetCacheDirectory(leftEyePath)))
        {
            yield return StartCoroutine(GetTexturesFromCache(leftEyePath, leftTextures));
        }
        else
        {
            yield return StartCoroutine(GetTexturesFromTif(leftEyePath, leftTextures));
        }

        if (Directory.Exists(GetCacheDirectory(rightEyePath)))
        {
            yield return StartCoroutine(GetTexturesFromCache(rightEyePath, rightTextures));
        }
        else
        {
            yield return StartCoroutine(GetTexturesFromTif(rightEyePath, rightTextures));
        }

        int leftTexSize = leftTextures[0].width;
        int rightTexSize = rightTextures[0].width;

        TextureFormat format = leftTextures[0].format;

        Cubemap leftCubemap = new Cubemap(leftTexSize, format, false);
        Cubemap rightCubemap = new Cubemap(rightTexSize, format, false);

        Debug.LogFormat("Left Tex Size: {0}", leftTexSize);
        Debug.LogFormat("Right Tex Size: {0}", rightTexSize);

        yield return StartCoroutine(CreateCubemapFromTextures(leftTextures, leftCubemap));
        yield return StartCoroutine(CreateCubemapFromTextures(rightTextures, rightCubemap));

        leftCubemap.Apply();
        rightCubemap.Apply();

        Debug.Log("Created Cubemaps");

        Debug.LogFormat("LEFT CUBEMAP: {0}", leftCubemap);

        yield return null;

        leftEye = new Material(Shader.Find("Skybox/Cubemap"));
        rightEye = new Material(Shader.Find("Skybox/Cubemap"));

        leftEye.SetTexture(Shader.PropertyToID("_Tex"), leftCubemap);
        rightEye.SetTexture(Shader.PropertyToID("_Tex"), rightCubemap);


        yield return null;

    }

    public void Update()
    {
        if (Input.GetKeyDown("l"))
        {
                    Camera.main.gameObject.AddComponent<Skybox>().material = leftEye;

        }
    }

    public IEnumerator GetTexturesFromCache(string filePath, List<Texture2D> textures)
    {

        string cacheDirectory = GetCacheDirectory(filePath);
        string fileName = Path.GetFileNameWithoutExtension(filePath);

        if (!Directory.Exists(cacheDirectory))
        {
            yield return StartCoroutine(GetTexturesFromTif(filePath, textures));
            yield break;
        }

        string[] facePaths = new string[6];

        string frontPath = cacheDirectory + "/" + fileName + "_front.png";
        string backPath = cacheDirectory + "/" + fileName + "_back.png";
        string leftPath = cacheDirectory + "/" + fileName + "_left.png";
        string rightPath = cacheDirectory + "/" + fileName + "_right.png";
        string upPath = cacheDirectory + "/" + fileName + "_up.png";
        string downPath = cacheDirectory + "/" + fileName + "_down.png";

        facePaths[FRONT_INDEX] = frontPath;
        facePaths[BACK_INDEX] = backPath;
        facePaths[LEFT_INDEX] = leftPath;
        facePaths[RIGHT_INDEX] = rightPath;
        facePaths[DOWN_INDEX] = downPath;
        facePaths[UP_INDEX] = upPath;

        textures.Clear();

        if (File.Exists(frontPath) && File.Exists(backPath) && File.Exists(leftPath) && File.Exists(rightPath) && File.Exists(upPath) && File.Exists(downPath))
        {

            for (int i = 0; i < 6; i++)
            {

                Debug.Log("Loading Texture " + i + " from cache");

                string fullPath = Path.GetFullPath(facePaths[i]);

                byte[] bytes = File.ReadAllBytes(fullPath);
                Texture2D newTex = new Texture2D(1, 1);
                newTex.LoadImage(bytes);

                yield return null;

                Debug.Log("Path is: " + fullPath);

                textures.Add(newTex);



            }
        }
        else
        {

            yield return StartCoroutine(GetTexturesFromTif(filePath, textures));
            yield break;

        }

        yield return null;
    }

    public IEnumerator GetTexturesFromTif(string tifPath, List<Texture2D> textures)
    {

        List<Image> images = new List<Image>();

        yield return StartCoroutine(LoadTifPages(tifPath, images));

        Debug.Log("Loaded Tif Images");

        yield return null;

        yield return StartCoroutine(LoadImagesAsTextures(images, textures));

        StartCoroutine(CacheTextures(textures, tifPath));

    }

    public static string GetCacheDirectory(string filePath)
    {

        string fileName = Path.GetFileNameWithoutExtension(filePath);

        string destinationFolder = cacheLocation + "/" + fileName;

        return destinationFolder;

    }

    public IEnumerator CacheTextures(List<Texture2D> textures, string imagePath)
    {

        string cacheDirectory = GetCacheDirectory(imagePath);

        if (Directory.Exists(cacheDirectory))
        {

            foreach (string file in Directory.GetFiles(cacheDirectory))
            {
                File.Delete(file);
            }

            Directory.Delete(cacheDirectory);
        }

        Directory.CreateDirectory(cacheDirectory);

        string fileName = Path.GetFileNameWithoutExtension(imagePath);

        string cachedPathFormat = cacheDirectory + "/" + fileName + "_{0}.png";



        for (int i = 0; i < textures.Count; i++)
        {

            string finalpath = "";

            switch (i)
            {
                case FRONT_INDEX:
                    finalpath = string.Format(cachedPathFormat, "front");
                    break;

                case BACK_INDEX:
                    finalpath = string.Format(cachedPathFormat, "back");
                    break;

                case LEFT_INDEX:
                    finalpath = string.Format(cachedPathFormat, "left");
                    break;

                case RIGHT_INDEX:
                    finalpath = string.Format(cachedPathFormat, "right");
                    break;

                case UP_INDEX:
                    finalpath = string.Format(cachedPathFormat, "up");
                    break;

                case DOWN_INDEX:
                    finalpath = string.Format(cachedPathFormat, "down");
                    break;
            }

            byte[] bytes = textures[i].EncodeToPNG();

            File.WriteAllBytes(finalpath, bytes);

        }

        yield return null;
    }

    public IEnumerator LoadTifPages(string imagePath, List<Image> tifImages)
    {

        Debug.Log("Loading Image As Textures: " + imagePath);

        TiffImage loadedTiff = new TiffImage(imagePath);

        yield return null;

        Debug.LogFormat("Getting individual tif pages from {0}", imagePath);

        for (int i = 0; i < loadedTiff.PageCount; i++)
        {

            tifImages.Add(loadedTiff.GetTiffSpecificPage(i));

            Debug.LogFormat("Loaded page {0} of the tif {1}", i, imagePath);

            yield return null;

        }
    }

    public IEnumerator LoadImagesAsTextures(List<Image> images, List<Texture2D> textures)
    {

        int pageNum = 0;

        foreach (Image img in images)
        {

            Bitmap bitmap = new Bitmap(img);

            int picWidth = bitmap.Width;
            int picHeight = bitmap.Height;

            Texture2D newTex = new Texture2D(picWidth, picHeight);

            Debug.LogFormat("Copying tif page {0} to Texture", pageNum);

            yield return null;

            for (int x = 0; x < picWidth; x++)
            {

                for (int y = 0; y < picHeight; y++)
                {

                    System.Drawing.Color color = bitmap.GetPixel(x, y);

                    UnityEngine.Color newColor = new UnityEngine.Color(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
                    newTex.SetPixel(x, y, newColor);

                }
            }

            Debug.LogFormat("Done copying tif page {0}", pageNum);

            pageNum++;

            yield return null;

            textures.Add(newTex);

        }
    }


    public IEnumerator CreateCubemapFromTextures(List<Texture2D> textures, Cubemap newCubemap)
    {
        Texture2D frontFace = textures[FRONT_INDEX];
        Texture2D backFace = textures[BACK_INDEX];

        Texture2D upFace = textures[UP_INDEX];
        Texture2D downFace = textures[DOWN_INDEX];

        Texture2D leftFace = textures[LEFT_INDEX];
        Texture2D rightFace = textures[RIGHT_INDEX];

        Debug.LogFormat("Setting Cubemap Faces from {0} textures", textures.Count);

        newCubemap.SetPixels(frontFace.GetPixels(), CubemapFace.PositiveZ);
        newCubemap.SetPixels(upFace.GetPixels(), CubemapFace.PositiveY);
        newCubemap.SetPixels(leftFace.GetPixels(), CubemapFace.NegativeX);
        newCubemap.SetPixels(rightFace.GetPixels(), CubemapFace.PositiveX);
        newCubemap.SetPixels(backFace.GetPixels(), CubemapFace.NegativeZ);
        newCubemap.SetPixels(downFace.GetPixels(), CubemapFace.NegativeY);

        yield return null;
    }


}


[System.Serializable]
public class SerializableCAVECam : SerializableCatalystDataType
{

    public string leftEyePath;
    public string rightEyePath;

    public SerializableCAVECam(string leftEye, string rightEye, string description)
    {

        this.leftEyePath = leftEye;
        this.rightEyePath = rightEye;
        this.description = description;

    }
}
