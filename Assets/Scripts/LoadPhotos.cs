using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LoadPhotos : MonoBehaviour {

    List<Sprite> sprites;
    List<CatalystPhoto> photos;

    List<Vector3> pictureLocations;


    public Vector2 finalImageSize = new Vector2(19.20f, 10.80f);
    public float uniformImageScale = 0.3f;

    private float picHeight;
    private float spawnOffset = 0;

    public string photoPath = "./Resources/PhotosToLoad";
    public Object photoPrefab;

    void Awake() {
        // load all images in sprites array
        //   sprites = LoadPNG();
       // photoPath = Application.dataPath + "/Resources/PicturesToLoad";
        photos = new List<CatalystPhoto>();
        StartCoroutine(LoadPhoto());
    }

    void Start() {
        // create the object
        GameObject photo = new GameObject();


        // add a "SpriteRenderer" component to the newly created object
        //photo.AddComponent<SpriteRenderer>();

        // assign the first loaded sprite to it
        //photo.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }

    /// <summary>
    /// Loads all pngs from the photos directory. Unknown if works with other file types? 
    /// </summary>
    /// <returns></returns>
    public List<Sprite> LoadPNG() {
        //Debug.Log(Directory.GetCurrentDirectory());
        string[] files = Directory.GetFiles(photoPath);
        List<Sprite> sprites = new List<Sprite>();

        Debug.Log("Number of files: " + files.Length);

        //For each file, load its data and convert to a sprite.
        for(int i = 0; i < files.Length; i++) {
            Texture2D tex = null;
            byte[] fileData;

            //Check if file exists and if its a png.
            if(File.Exists(files[i]) && Path.GetExtension(files[i]) == ".png") {
                Debug.Log("Loading: " + files[i]);
                fileData = File.ReadAllBytes(files[i]);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

                //Convert texture to sprite.
                sprites.Add(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2()));
            }
        }
        return sprites;
    }

    void LoadPictureLocations(float numPics) {

        float spawnSphereRadius = 10.0f;
        pictureLocations = new List<Vector3>();


        if(numPics == 1) {

            Vector3 spawnLoc = new Vector3(1, 0, 0);
            spawnLoc *= spawnSphereRadius;
            pictureLocations.Add(spawnLoc);

        }

        else if(numPics == 2) {

            //only spawn two pictures

        }

        else if(numPics == 3) {

            //three pic case
        }

        else if(numPics == 4) {

        }

        else if(numPics == 5) {

        }

        else if(numPics >= 8) {

            float numRows = Mathf.Ceil(numPics / 8);

            float rad2o2 = Mathf.Sqrt(2) / 2;

            pictureLocations.Add(new Vector3(0, 0, 1));
            pictureLocations.Add(new Vector3(rad2o2, 0, rad2o2));
            pictureLocations.Add(new Vector3(1, 0, 0));
            pictureLocations.Add(new Vector3(rad2o2, 0, -rad2o2));
            pictureLocations.Add(new Vector3(0, 0, -1));
            pictureLocations.Add(new Vector3(-rad2o2, 0, -rad2o2));
            pictureLocations.Add(new Vector3(-1, 0, 0));
            pictureLocations.Add(new Vector3(-rad2o2, 0, rad2o2));

            for(int i = 0; i < pictureLocations.Count; i++) {

                pictureLocations[i] *= spawnSphereRadius;

            }

            int j = 0;
            float yVal = 0;
            float pictureHeight = finalImageSize.y * uniformImageScale;

            for(int i = 0; i < photos.Count; i++) {

                if(i % 8 == 0) {


                    if(Mathf.Ceil(i / 8) % 2 == 0) {

                        yVal = -yVal;
                    }


                    else {
                        yVal = -yVal;
                        yVal += pictureHeight + (pictureHeight / 4);
                    }
                }

                if(i < pictureLocations.Count) {

                    j = i;

                }
                else {
                    j++;
                }

                if(j > pictureLocations.Count - 1) j = 0;

                Vector3 placeVector = pictureLocations[j];
                placeVector.y = yVal;

                photos[i].transform.position = placeVector;

            }
        }
    }

    Vector3 GetValidPictureLocation() {

        float numPhotos = photos.Count;
        float photoSphereRadius = 30.0f;
        float theta = 0, phi = 0;



        // theta = 2 * Mathf.PI * Random.Range(0f, 1f);
        phi = Mathf.Acos((2f * Random.Range(0f, 1f)) - 1f);

        Vector3 sphereCoords = new Vector3(
          Mathf.Cos(theta) * Mathf.Sin(phi),
          Mathf.Sin(theta) * Mathf.Sin(phi),
          Mathf.Cos(phi));

        sphereCoords *= photoSphereRadius;
        sphereCoords += Camera.main.transform.position;

        return sphereCoords;



    }

    IEnumerator LoadPhoto() {

        List<Sprite> imageSprites = new List<Sprite>();
        List<string> imageFilePaths = new List<string>();

        //directory to load files from
        string[] files = Directory.GetFiles(photoPath);

        //loops through all files in the directory
        foreach(string str in files) {

            //checks the extension on the file
            if(Path.GetExtension(str) == ".png" || Path.GetExtension(str) == ".jpg") {

                imageFilePaths.Add(str);

            }

            else {

                Debug.LogWarning("File extension " + Path.GetExtension(str) + " is not supported");
            }
        }

        //Creates in-game textures for the various images
        for(int i = 0; i < imageFilePaths.Count; i++) {

            string str = imageFilePaths[i];

            //updates the path to load a WWW
            string imageString = "file://" + str;

            //loads the WWW (Whatever that means)
            WWW w = new WWW(imageString);

            //waits for the WWW to finish loading
            while(!w.isDone) {

                yield return null;

            }

            Debug.Log("Loading Image " + i + " of " + imageFilePaths.Count);

            //Gets the audio clip from the WWW and adds to list
            Texture2D image = new Texture2D(1, 1);
            w.LoadImageIntoTexture(image);
            Sprite imageSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            imageSprites.Add(imageSprite);
            Debug.Log("IMAGE SIZE: " + imageSprite.bounds.size.x + ", " + imageSprite.bounds.size.y);
            yield return null;
        }

        for (int i = 0; i < imageSprites.Count; i++) {

            Vector2 finalImageSize = new Vector2(19.20f, 10.80f);
            Vector2 currentImageSize = new Vector2(imageSprites[i].bounds.size.x, imageSprites[i].bounds.size.y);
            Vector3 scaleFactor = new Vector3(finalImageSize.x / currentImageSize.x, finalImageSize.y / currentImageSize.y);

            Debug.Log("Instantiating Image " + i + " of " + imageSprites.Count);
            GameObject photoObj = (GameObject)GameObject.Instantiate(photoPrefab, GetValidPictureLocation(), new Quaternion());
            photoObj.GetComponent<SpriteRenderer>().sprite = imageSprites[i];
            Vector3 photoScale = photoObj.transform.localScale;

            photoScale.x = -photoScale.x;
            photoScale.x *= scaleFactor.x;
            photoScale.y *= scaleFactor.y;

            photoScale *= uniformImageScale;

            photoObj.transform.localScale = photoScale;
            photoObj.GetComponent<BoxCollider>().size = photoObj.GetComponent<SpriteRenderer>().sprite.bounds.size;

           // photoObj.GetComponent<BoxCollider>().size = finalImageSize;
            //Debug.Log("COLLIDER SIZE: " + photoObj.GetComponent<BoxCollider>().size);

            photos.Add(photoObj.GetComponent<CatalystPhoto>());

            yield return null;

        }

        LoadPictureLocations(imageSprites.Count);

        foreach(CatalystPhoto photo in photos) {

            photo.gameObject.SetActive(true);

        }
    }
}
