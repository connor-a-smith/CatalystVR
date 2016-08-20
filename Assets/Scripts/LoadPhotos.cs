using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Method that loads photos given a directory to load from, and creates them around a user.
/// </summary>
public class LoadPhotos : MonoBehaviour {

    private List<Sprite> sprites;

    // List of actual spawned Catalyst Photos
    private List<CatalystPhoto> photos;

    // List that will store where each photo should go around the user.
    private List<Vector3> pictureLocations;

    // Maximum number of pictures that can be loaded. Max is 24 (3 rows of 8).
    [SerializeField] private int numPicsToLoad = 24;

    // Resolution of the image after it's created and loaded from file.
    // We use 1920x1080 for now (19.20, 10.80) for 16:9 ratio.
    [SerializeField] private Vector2 finalImageSize = new Vector2(19.20f, 10.80f);

    // Resizes the picture uniformly, so that we can make it smaller be keep easy 16:9 ration.
    [SerializeField] private float uniformImageScale = 0.3f;

    // How far the pictures should be from the user.
    [SerializeField] private float spawnSphereRadius = 10.0f;

    private float picHeight;
    private float spawnOffset = 0;

    // Directory to load pictures from. Will only load first numPicsToLoad amount.
    // NOTE: This is local to the assets folder
    public string photoPath = "/Resources/PhotosToLoad";

    // Predefined prefab for a photo.
    private Object photoPrefab;

    // Values to store how many pictures should go in top/bottom rows and middle row.
    private int picsOnTopAndBottom;
    private int picsInMiddle;

    // Whether or not the pictures should load on game start.
    public bool loadOnStart = false;

    // Object to hold all the photos so they don't make the hierarchy messy.
    // Can drag in your own gameobject, but not required. One will be created if none dragged.
    [SerializeField] private GameObject photoHolder = null;

    // Initialization
    void Start() {

        // Creates a photo holder if one doesn't already exist.
        if(photoHolder == null) {
            photoHolder = new GameObject();
        }
  
        // Grabs a static photo prefab from the game's controller.
        photoPrefab = Controller.photoPrefab;

        // Actually load the pictures.
        if (loadOnStart)
        {
            Load();
        }
    }

    /// <summary>
    /// Launching coroutine here instead of awake so that the loading can begin after the correct path is set.
    /// </summary>
    public void Load()
    {
        // Gets path to load from.
        photoPath = Application.dataPath + photoPath;

        // Creates a list of photos.
        photos = new List<CatalystPhoto>();

        // Actually loads the photos.
        StartCoroutine(LoadPhoto());
    }

    /// <summary>
    /// Calculates the positions for each photo that will be loaded.
    /// </summary>
    /// <param name="numPics"> How many pictures will be loaded </param>
    void LoadPictureLocations(int numPics) {

        // How tall the picture should be.
        float pictureHeight = finalImageSize.y * uniformImageScale;

        // Initializes picture locations list.
        pictureLocations = new List<Vector3>();

        // Just stores radical(2) / 2, for use with unit circle calculations.
        float rad2o2 = Mathf.Sqrt(2.0f) / 2.0f;

        // List of positions on the unit circle to make life easier.
        List<List<Vector3>> unitCircleLocations = new List<List<Vector3>>();

        // Calculates positions on unit circle and stores them into vectors.
        // Keep in mind that in 2D, we use cos(x) and sin(y). In 3D here, we use cos(x) and sin(z)!
        Vector3 unit0 = new Vector3(1, 0, 0);
        Vector3 unit45 = new Vector3(rad2o2, 0, rad2o2);
        Vector3 unit90 = new Vector3(0, 0, 1);
        Vector3 unit135 = new Vector3(-rad2o2, 0, rad2o2);
        Vector3 unit60 = new Vector3(Mathf.Cos(2 * Mathf.PI / 6), 0, Mathf.Sin(2 * Mathf.PI / 6));
        Vector3 unit105 = new Vector3(-Mathf.Cos(2 * Mathf.PI / 6), 0, Mathf.Sin(2 * Mathf.PI / 6));

        // Special positions for 1, 2, and 3 pictures, since they won't be rotating or spaced evenly.
        unitCircleLocations.Add(new List<Vector3> { unit90 });
        unitCircleLocations.Add(new List<Vector3> { unit60, unit105 });
        unitCircleLocations.Add(new List<Vector3> { unit90, unit45, unit135 });

        for(int i = 3; i < 8; i++) {

            // Initialize list at i position.
            unitCircleLocations.Add(new List<Vector3>());

            // Calculates the positions based on number of sides for shape of i sides.
            unitCircleLocations[i] = GetPositionsOnUnitCircleBySides(i + 1);

        }

        // If there are more than 8 pictures, we have to consider multiple rows.
        if(numPics > 8) {

            // If the number of pictures is even divisible by 3, same number of pics on all rows.
            if(numPics % 3 == 0) {
                // Just divide number of pictures by 3 to get how many should be on each row.
                picsOnTopAndBottom = (int)numPics / 3;
                picsInMiddle = picsOnTopAndBottom;
            }

            // If the total number of pictures doesn't divide easily by 3, do some extra math.
            else {

                // Number of pics on top and bottom are just the number divided by 3.
                picsOnTopAndBottom = (int)numPics / 3;

                // Number of pictures in middle row is just all remaining pictures.
                picsInMiddle = (int)numPics - (2 * picsOnTopAndBottom);
            }

            //iterates through each row of pictures
            for(int i = 0; i < 3; i++) {

                //middle row
                if(i == 0) {

                    // Stores the picture locations for the middle row, iterating through each location.
                    for(int j = 0; j < unitCircleLocations[picsInMiddle - 1].Count; j++) {

                        // Grabs the location of each picture based on the number of pictures that row has.
                        // Why does this work? Well, unitCircleLocations[sideNumber-1] grabs positions for
                        // a shape of sideNumber sides. Our pictures are essentially our "sides", and so 
                        // their arrangement is determined by the math we did earlier and stored into
                        // unitCircleLocations.
                        Vector3 pictureLocation = unitCircleLocations[picsInMiddle - 1][j];

                        // Offsets the picture.
                        pictureLocation *= spawnSphereRadius;

                        // Stores the final picture locations
                        pictureLocations.Add(pictureLocation);
                    }
                }

                //top and bottom rows
                else {

                    // Does the same math as above, but for top and bottom rows.
                    for(int j = 0; j < unitCircleLocations[picsOnTopAndBottom - 1].Count; j++) {

                        Vector3 pictureLocation = unitCircleLocations[picsOnTopAndBottom - 1][j];

                        pictureLocation *= spawnSphereRadius;

                        // Separate cases for moving the pictures up and down, depending on rows.
                        if(i == 1) pictureLocation.y += (pictureHeight + pictureHeight / 4);
                        if(i == 2) pictureLocation.y -= (pictureHeight + pictureHeight / 4);

                        // Adds final picture locations.
                        pictureLocations.Add(pictureLocation);

                    }
                }
            }
        }

        //special cases for number < 8
        else {

            // There's only going to be one row, so just store total num as pics in middle.
            picsInMiddle = numPics;
            pictureLocations = unitCircleLocations[numPics - 1];

            // Offset all the positions of the picture locations to be around camera.
            for(int j = 0; j < pictureLocations.Count; j++) {

                pictureLocations[j] *= spawnSphereRadius;
            }
        }
    }

    /// <summary>
    /// Loads a picture from file.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadPhoto() {

        List<Sprite> imageSprites = new List<Sprite>();
        List<string> imageFilePaths = new List<string>();

        //directory to load files from
        string[] files = Directory.GetFiles(photoPath);

        //loops through all files in the directory
        foreach(string str in files) {

            //checks the extension on the file
            if(Path.GetExtension(str) == ".png" || Path.GetExtension(str) == ".jpg" || Path.GetExtension(str) == ".JPG") {

                // If the number of pictures in the directory exceeds max pictures supported
                if (imageFilePaths.Count >= numPicsToLoad) {

                    // Log a warning and stop the loop. No point in loading more pictures at this point.
                    Debug.LogWarning("Number of pictures in directory exceeds " + numPicsToLoad + ". Only first " + numPicsToLoad + " will be used");
                    break;
                }

                // Add the image path to the list of file paths.
                imageFilePaths.Add(str);

            }

            // If this file type is not supported, print a warning and skip it.
            else {

                Debug.LogWarning("File extension " + Path.GetExtension(str) + " is not supported. Please convert before attempting to load.");
            }
        }

        //Creates in-game textures for the various images
        for(int i = 0; i < imageFilePaths.Count; i++) {

            // Stores the file path
            string str = imageFilePaths[i];

            // Updates the path to load a WWW from local computer
            string imageString = "file://" + str;

            //loads the WWW (Whatever that means)
            WWW w = new WWW(imageString);

            // Waits for the WWW to finish loading
            while(!w.isDone) {

                // Waits for next frame
                yield return null;

            }

            // Prints out a log to give some sense of progress for the image loading.
            // Waiting is no fun, so at least you can see how soon it'll be ready!
            Debug.Log("Loading Image " + i + " of " + imageFilePaths.Count);

            // >>> NOTE <<< For future reference, THIS is the part that takes so long!
            // Recommend some sort of loading bar/screen for future use.
            // Game will be crazy laggy while this is loading.
            
            // Gets the audio clip from the WWW and adds to list
            Texture2D image = new Texture2D(1, 1);

            // Loads the image from file into an in-game texture.
            w.LoadImageIntoTexture(image);

            // Creates a new in-game sprite and populates it with the new texture.
            Sprite imageSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            
            // Add the final sprite into a texture.
            imageSprites.Add(imageSprite);

            // Wait for the next frame to load another picture.
            // Honestly we only do this so that the Loading Image debug works.
            // Otherwise game will freeze completely until all pictures loaded.
            yield return null;
        }

        // Loads the picture locations for the loaded images, based on number of pictures.
        LoadPictureLocations(imageSprites.Count);

        // Loops through each loaded picture 
        for(int i = 0; i < imageSprites.Count; i++) {

            // Calculates size of picture.
            Vector2 currentImageSize = new Vector2(imageSprites[i].bounds.size.x, imageSprites[i].bounds.size.y);

            // Determines how much the photo needs to be scaled by to match final size.
            Vector3 scaleFactor = new Vector3(finalImageSize.x / currentImageSize.x, finalImageSize.y / currentImageSize.y);

            Debug.Log("Instantiating Image " + i + " of " + imageSprites.Count);

            // Instantiates the image in the scene.
            GameObject photoObj = (GameObject)GameObject.Instantiate(photoPrefab, pictureLocations[i], new Quaternion());

            // Places the sprite in the gameObject
            photoObj.GetComponent<SpriteRenderer>().sprite = imageSprites[i];

            // Stores current scale of the object.
            Vector3 photoScale = photoObj.transform.localScale;

            // Reflects the image, because for some reason they spawn backwards
            photoScale.x = -photoScale.x;

            // Scale the images by the calculated scale factor, to make them match correct ratio.
            photoScale.x *= scaleFactor.x;
            photoScale.y *= scaleFactor.y;

            // Uniformly resizes the image.
            photoScale *= uniformImageScale;

            // Resets the scale on the photo, should be correct ratio now.
            photoObj.transform.localScale = photoScale;

            // Adds a collider to the photo for selection.
            photoObj.GetComponent<BoxCollider>().size = photoObj.GetComponent<SpriteRenderer>().sprite.bounds.size;

            // Adds the photo to the final list of photos.
            photos.Add(photoObj.GetComponent<CatalystPhoto>());

            // Wait for next frame to load next photo.
            yield return null;

        }

        // Place the photos in the correct positions around the user.
        PlacePhotos();

        // Sets whether or not each photo should be spinning.
        for(int i = 0; i < photos.Count; i++) {

            // If there's more than 3 pictures in the middle, and the current index is a middle picture.
            if (picsInMiddle > 3 && i < picsInMiddle) {

                // Then this photo should be spinning.
                photos[i].isSpinning = true;

            }

            // If there's more than 3 pictures on top/bottom, and the current index is a top/bottom picture.
            if (picsOnTopAndBottom > 3 && i >= picsInMiddle) {

                // Then this photo should be spinning.
                photos[i].isSpinning = true;

            }

            // Finally activates the object.
            photos[i].gameObject.SetActive(true);
        }

        // Used to hide every image when all images are loaded.
        // Do this if the pictures should load but not show immediately.
        if (!loadOnStart)
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Uses math to determine unit circle positions for a shape of certain sides.
    /// </summary>
    /// <param name="numSides"> Number of sides the shape to calculate for has </param>
    /// <returns> A list of 3D Vector3 coordinates for each point on the unit circle </returns>
    List<Vector3> GetPositionsOnUnitCircleBySides(int numSides) {

        // Creates a list to return.
        List<Vector3> returnList = new List<Vector3>();

        // Math works. Always believe. Genius Credit: Anish
        for(int i = 0; i < numSides; i++) {
            float radAngle = i * (2 * (Mathf.PI / numSides));
            returnList.Add(new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle)));

        }

        // Returns the list.
        return returnList;
    }
    
    // Places the photos around user.
    public void PlacePhotos() {

        // Places the photo holder (Center) directly at camera position.
        photoHolder.transform.position = Camera.main.transform.position;

        // Places photo holder under current gameObject.
        photoHolder.transform.parent = this.transform;

        for(int i = 0; i < photos.Count; i++) {

            // Places photo under the photoHolder.
            photos[i].transform.parent = photoHolder.transform;

            // Updates the position of the photo to new location (Should be centered around camera).
            photos[i].transform.localPosition = pictureLocations[i];
        }
    }
}
