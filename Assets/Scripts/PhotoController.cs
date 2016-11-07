using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Method that loads photos given a directory to load from, and creates them around a user.
/// </summary>
public class PhotoController : MonoBehaviour
{

    private bool movingPhotos = false;

    private List<Sprite> sprites;

    [SerializeField]
    private bool debug;

    public CatalystPhoto activePhoto;

    // List of actual spawned Catalyst Photos
    private List<List<CatalystPhoto>> photos;

    // List that will store where each photo should go around the user.
    private List<List<Vector3>> pictureLocations;
    private float photoYOffset;

    private float angleBetweenPhotos;

    // Maximum number of pictures that can be loaded. Max is 24 (3 rows of 8).
    public int numPicsToLoad = 24;

    // Resolution of the image after it's created and loaded from file.
    // We use 1920x1080 for now (19.20, 10.80) for 16:9 ratio.
    private Vector2 finalImageSize = new Vector2(19.20f, 10.80f);

    // Resizes the picture uniformly, so that we can make it smaller be keep easy 16:9 ration.
    private float uniformImageScale = .8f;

    // How far the pictures should be from the user.
    private float spawnSphereRadius = 22.0f;

    private float picHeight;

    // Directory to load pictures from. Will only load first numPicsToLoad amount.
    // NOTE: This is local to the assets folde
    public bool pathIsExteral = false;
    public string photoPath = "PhotosToLoad";

    // Predefined prefab for a photo.
    private Object photoPrefab;

    // Values to store how many pictures should go in top/bottom rows and middle row.
    private int picsOnTopAndBottom;
    private int picsInMiddle;

    // Whether or not the pictures should load on game start.
    public bool loadOnStart = false;

    // Object to hold all the photos so they don't make the hierarchy messy.
    // Can drag in your own gameobject, but not required. One will be created if none dragged.
    [SerializeField]
    public GameObject photoHolder = null;
    [SerializeField]
    private GameObject photoCenter = null;

    // Initialization
    void Start()
    {

        if (photoPrefab == null)
        {

            photoPrefab = new GameObject();
            ((GameObject)photoPrefab).AddComponent<CatalystPhoto>();
            ((GameObject)photoPrefab).AddComponent<SpriteRenderer>();
            ((GameObject)photoPrefab).AddComponent<BoxCollider>();

        }


        if (photoCenter == null)
        {

            photoCenter = Controller.instance.raycastCam.gameObject;

        }


        // Creates a photo holder if one doesn't already exist.
        if (photoHolder == null)
        {
            photoHolder = new GameObject();
            photoHolder.name = "Photo Holder";
            photoHolder.transform.parent = photoCenter.transform;
            photoHolder.transform.localPosition = Vector3.zero;
            photoHolder.transform.localRotation = Quaternion.identity;
        }

        // Grabs a static photo prefab from the game's controller.
        //photoPrefab = Controller.photoPrefab;

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
        //photoPath = Application.dataPath + photoPath;

        // Creates a list of photos.
        photos = new List<List<CatalystPhoto>>();

        // Actually loads the photos.
        StartCoroutine(LoadPhoto());
    }

    /// <summary>
    /// Calculates the positions for each photo that will be loaded.
    /// </summary>
    /// <param name="numPics"> How many pictures will be loaded </param>
    void LoadPictureLocations(int numPics)
    {

        if (debug)
        {

            Debug.Log("Loading picture locations");

        }

        // How tall the picture should be.
        float pictureHeight = finalImageSize.y * uniformImageScale;
        photoYOffset = pictureHeight + (pictureHeight / 4);

        // Initializes picture locations list.
        pictureLocations = new List<List<Vector3>>();

        // Just stores radical(2) / 2, for use with unit circle calculations.
        float rad2o2 = Mathf.Sqrt(2.0f) / 2.0f;

        // List of positions on the unit circle to make life easier.
        List<List<Vector3>> unitCircleLocations = new List<List<Vector3>>();

        // Calculates positions on unit circle and stores them into vectors.
        // Keep in mind that in 2D, we use cos(x) and sin(y). In 3D here, we use cos(x) and sin(z)!
        Vector3 unit45 = new Vector3(rad2o2, 0, rad2o2);
        Vector3 unit90 = new Vector3(0, 0, 1);
        Vector3 unit135 = new Vector3(-rad2o2, 0, rad2o2);
        Vector3 unit60 = new Vector3(Mathf.Cos(2 * Mathf.PI / 6), 0, Mathf.Sin(2 * Mathf.PI / 6));
        Vector3 unit105 = new Vector3(-Mathf.Cos(2 * Mathf.PI / 6), 0, Mathf.Sin(2 * Mathf.PI / 6));

        // Special positions for 1, 2, and 3 pictures, since they won't be rotating or spaced evenly.
        unitCircleLocations.Add(new List<Vector3> { unit90 });
        unitCircleLocations.Add(new List<Vector3> { unit90, unit135 });
        unitCircleLocations.Add(new List<Vector3> { unit90, unit45, unit135 });

        for (int i = 3; i < 8; i++)
        {

            // Initialize list at i position.
            unitCircleLocations.Add(new List<Vector3>());

            // Calculates the positions based on number of sides for shape of i sides.
            unitCircleLocations[i] = GetPositionsOnUnitCircleBySides(i + 1);

        }

        // If there are more than 8 pictures, we have to consider multiple rows.
        if (numPics > 8)
        {

            if (debug)
            {

                Debug.Log("More than 8 pictures found, will load 3 rows");

            }

            for (int i = 0; i < 3; i++)
            {

                pictureLocations.Add(new List<Vector3>());

            }

            // If the number of pictures is even divisible by 3, same number of pics on all rows.
            if (numPics % 3 == 0)
            {
                // Just divide number of pictures by 3 to get how many should be on each row.
                picsOnTopAndBottom = (int)numPics / 3;
                picsInMiddle = picsOnTopAndBottom;

            }

            // If the total number of pictures doesn't divide easily by 3, do some extra math.
            else
            {

                // Number of pics on top and bottom are just the number divided by 3.
                picsOnTopAndBottom = (int)numPics / 3;

                // Number of pictures in middle row is just all remaining pictures.
                picsInMiddle = (int)numPics - (2 * picsOnTopAndBottom);

            }

            //iterates through each row of pictures
            for (int row = 0; row < 3; row++)
            {

                if (row != 1)
                {

                    // Does the same math as above, but for top and bottom rows.
                    for (int col = 0; col < unitCircleLocations[picsOnTopAndBottom - 1].Count; col++)
                    {

                        Vector3 pictureLocation = unitCircleLocations[picsOnTopAndBottom - 1][col];

                        pictureLocation *= spawnSphereRadius;

                        // Separate cases for moving the pictures up and down, depending on rows.
                        if (row == 0)
                            pictureLocation.y += photoYOffset;
                        if (row == 2)
                            pictureLocation.y -= photoYOffset;

                        // Adds final picture locations.
                        pictureLocations[row].Add(pictureLocation);
                    }

                }
                else
                {

                    // Stores the picture locations for the middle row, iterating through each location.
                    for (int col = 0; col < unitCircleLocations[picsInMiddle - 1].Count; col++)
                    {

                        // Grabs the location of each picture based on the number of pictures that row has.
                        // Why does this work? Well, unitCircleLocations[sideNumber-1] grabs positions for
                        // a shape of sideNumber sides. Our pictures are essentially our "sides", and so 
                        // their arrangement is determined by the math we did earlier and stored into
                        // unitCircleLocations.
                        Vector3 pictureLocation = unitCircleLocations[picsInMiddle - 1][col];

                        // Offsets the picture.
                        pictureLocation *= spawnSphereRadius;

                        // Stores the final picture locations
                        pictureLocations[row].Add(pictureLocation);

                    }
                }
            }
        }

        //special cases for number < 8
        else
        {

            pictureLocations.Add(new List<Vector3>());

            if (debug)
            {

                Debug.Log("Less than 8 pictures found, will only load one row");

            }

            // There's only going to be one row, so just store total num as pics in middle.
            picsInMiddle = numPics;
            pictureLocations[0] = unitCircleLocations[numPics - 1];



            // Offset all the positions of the picture locations to be around camera.
            for (int col = 0; col < pictureLocations[0].Count; col++)
            {

                pictureLocations[0][col] *= spawnSphereRadius;

            }
        }
    }

    /// <summary>
    /// Loads a picture from file.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadPhoto()
    {

        List<Sprite> imageSprites = new List<Sprite>();

        if (pathIsExteral)
        {

            yield return StartCoroutine(LoadImagesExtern(imageSprites));

        }
        else
        {

            yield return StartCoroutine(LoadImagesIntern(imageSprites));

        }

        if (imageSprites.Count > 0)
        {

            // Loads the picture locations for the loaded images, based on number of pictures.
            LoadPictureLocations(imageSprites.Count);

            int imgNum = 0;

            // Loops through each loaded picture 
            for (int row = 0; row < pictureLocations.Count; row++)
            {

                photos.Add(new List<CatalystPhoto>());

                for (int col = 0; col < pictureLocations[row].Count; col++)
                {

                    // Calculates size of picture.
                    Vector2 currentImageSize = new Vector2(imageSprites[imgNum].bounds.size.x, imageSprites[imgNum].bounds.size.y);

                    // Determines how much the photo needs to be scaled by to match final size.
                    Vector3 scaleFactor = new Vector3(finalImageSize.x / currentImageSize.x, finalImageSize.y / currentImageSize.y);

                    Debug.Log("Instantiating Image " + imgNum + " of " + imageSprites.Count);

                    // Instantiates the image in the scene.
                    GameObject photoObj = (GameObject)GameObject.Instantiate(photoPrefab, pictureLocations[row][col], new Quaternion());

                    // Places the sprite in the gameObject
                    photoObj.GetComponent<SpriteRenderer>().sprite = imageSprites[imgNum];

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
                    photos[row].Add(photoObj.GetComponent<CatalystPhoto>());

                    float angleBetweenPhotos = 360.0f / pictureLocations[row].Count;

                    if (pictureLocations[row].Count < 4)
                    {

                        angleBetweenPhotos = 45.0f;

                    }

                    photos[row][col].SetPhotoInfo(row, col, angleBetweenPhotos);

                    imgNum++;

                    // Wait for next frame to load next photo.
                    yield return null;
                }
            }

            // Place the photos in the correct positions around the user.
            PlacePhotos();

            if (photos.Count > 1)
            {
                activePhoto = photos[1][0];
            }
            else
            {
                activePhoto = photos[0][0];
            }


            for (int row = 0; row < photos.Count; row++)
            {
                for (int col = 0; col < photos[row].Count; col++)
                {

                    photos[row][col].gameObject.SetActive(true);

                }
            }
        }
        else
        {

            Debug.LogWarningFormat("Unable to load {0} pictures from \"{1}\". Check the load number or path.", numPicsToLoad, photoPath);

        }
    }

    public IEnumerator LoadImagesExtern(List<Sprite> spriteList)
    {

        List<string> imageFilePaths = new List<string>();

        //directory to load files from
        string[] files = Directory.GetFiles(photoPath);

        if (files != null && files.Length > 0)
        {

            //loops through all files in the directory
            foreach (string str in files)
            {

                //checks the extension on the file
                if (Path.GetExtension(str) == ".png" || Path.GetExtension(str) == ".jpg" || Path.GetExtension(str) == ".JPG")
                {

                    // If the number of pictures in the directory exceeds max pictures supported
                    if (imageFilePaths.Count >= numPicsToLoad)
                    {

                        // Log a warning and stop the loop. No point in loading more pictures at this point.
                        Debug.LogWarning("Number of pictures in directory exceeds " + numPicsToLoad + ". Only first " + numPicsToLoad + " will be used");
                        break;
                    }

                    // Add the image path to the list of file paths.
                    imageFilePaths.Add(str);

                }

                // If this file type is not supported, print a warning and skip it.
                else
                {

                    Debug.LogWarning("File extension " + Path.GetExtension(str) + " is not supported. Please convert before attempting to load.");
                }
            }

            //Creates in-game textures for the various images
            for (int i = 0; i < imageFilePaths.Count; i++)
            {

                // Stores the file path
                string str = imageFilePaths[i];

                // Updates the path to load a WWW from local computer
                string imageString = "file://" + str;

                //loads the WWW (Whatever that means)
                WWW w = new WWW(imageString);

                // Waits for the WWW to finish loading
                while (!w.isDone)
                {

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
                spriteList.Add(imageSprite);

                // Wait for the next frame to load another picture.
                // Honestly we only do this so that the Loading Image debug works.
                // Otherwise game will freeze completely until all pictures loaded.
                yield return null;
            }
        }
    }

    public IEnumerator LoadImagesIntern(List<Sprite> spriteList)
    {

        Object[] photoFiles = Resources.LoadAll(photoPath);

        foreach (Object photo in photoFiles)
        {

            if (spriteList.Count < numPicsToLoad)
            {

                Texture2D image = photo as Texture2D;

                if (image != null)
                {

                    // Creates a new in-game sprite and populates it with the new texture.
                    Sprite imageSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));

                    // Add the final sprite into a texture.
                    spriteList.Add(imageSprite);

                    yield return null;
                }
            }
            else
            {

                Debug.LogWarning("Number of pictures in directory exceeds " + numPicsToLoad + ". Only first " + numPicsToLoad + " will be used");
                break;

            }
        }
    }

    /// <summary>
    /// Uses math to determine unit circle positions for a shape of certain sides.
    /// </summary>
    /// <param name="numSides"> Number of sides the shape to calculate for has </param>
    /// <returns> A list of 3D Vector3 coordinates for each point on the unit circle </returns>
    List<Vector3> GetPositionsOnUnitCircleBySides(int numSides)
    {

        // Creates a list to return.
        List<Vector3> returnList = new List<Vector3>();

        // Math works. Always believe. Genius Credit: Anish
        for (int i = 0; i < numSides; i++)
        {
            float radAngle = i * (2 * (Mathf.PI / numSides));
            radAngle += (Mathf.PI / 2);
            returnList.Add(new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle)));
        }



        // Returns the list.
        return returnList;
    }

    // Places the photos around user.
    public void PlacePhotos()
    {

        // Places the photo holder (Center) directly at camera position.
        photoHolder.transform.position = photoCenter.transform.position;

        for (int row = 0; row < photos.Count; row++)
        {
            for (int col = 0; col < photos[row].Count; col++)
            {

                photos[row][col].transform.parent = photoHolder.transform;
                photos[row][col].transform.localPosition = pictureLocations[row][col];

            }
        }
    }

    public void MoveRight()
    {

        if (!movingPhotos)
        {
            float angleToRotate = activePhoto.angleBetweenPhotos;
            Vector3 rotationVector = new Vector3(0.0f, -angleToRotate, 0.0f);

            Debug.Log("Angle: " + angleToRotate);


            StartCoroutine((Rotate(rotationVector, 0.25f)));

            int newActiveRow = activePhoto.row;
            int newActiveCol = activePhoto.col - 1;

            if (newActiveCol < 0)
            {

                newActiveCol = pictureLocations[newActiveRow].Count - 1;

            }

            activePhoto = photos[newActiveRow][newActiveCol];
        }
    }

    public void MoveLeft()
    {

        if (!movingPhotos)
        {
            float angleToRotate = activePhoto.angleBetweenPhotos;
            Vector3 rotationVector = new Vector3(0.0f, angleToRotate, 0.0f);

            Debug.Log("Angle: " + angleToRotate);
            StartCoroutine((Rotate(rotationVector, 0.25f)));

            int newActiveRow = activePhoto.row;
            int newActiveCol = activePhoto.col + 1;

            if (newActiveCol >= pictureLocations[newActiveRow].Count)
            {

                newActiveCol = 0;

            }

            activePhoto = photos[newActiveRow][newActiveCol];
        }
    }

    public void MoveUp()
    {

        if (!movingPhotos)
        {
            Vector3 moveVector = new Vector3(0.0f, -photoYOffset, 0.0f);

            int activePhotoNewRow = activePhoto.row - 1;

            if (activePhotoNewRow >= 0)
            {

                float photoFraction = activePhoto.col / (float)pictureLocations[activePhoto.row].Count;

                int activePhotoNewCol = (int)Mathf.Floor(pictureLocations[activePhotoNewRow].Count * photoFraction);
                CatalystPhoto newActivePhoto = photos[activePhotoNewRow][activePhotoNewCol];
                float angleDiff = Mathf.Abs(activePhoto.angleBetweenPhotos - newActivePhoto.angleBetweenPhotos);

                Vector3 rotateVector = new Vector3(0.0f, angleDiff, 0.0f);

                StartCoroutine(MoveAndRotate(moveVector, rotateVector, 0.25f));

                activePhoto = newActivePhoto;

            }
        }
    }

    public void MoveDown()
    {

        if (!movingPhotos)
        {
            Debug.Log("Active row was: " + activePhoto.row);


            Vector3 moveVector = new Vector3(0.0f, photoYOffset, 0.0f);

            int activePhotoNewRow = activePhoto.row + 1;


            if (activePhotoNewRow < pictureLocations.Count)
            {

                Debug.Log("Moving to row " + activePhotoNewRow);


                float photoFraction = activePhoto.col / (float)pictureLocations[activePhoto.row].Count;
                int activePhotoNewCol = (int)(pictureLocations[activePhotoNewRow].Count * photoFraction);
                CatalystPhoto newActivePhoto = photos[activePhotoNewRow][activePhotoNewCol];
                float angleDiff = Mathf.Abs(activePhoto.angleBetweenPhotos - newActivePhoto.angleBetweenPhotos);

                Vector3 rotateVector = new Vector3(0.0f, angleDiff, 0.0f);

                StartCoroutine(MoveAndRotate(moveVector, rotateVector, 0.25f));

                activePhoto = newActivePhoto;

            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("l");

            MoveLeft();

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();

        }
        else if (Input.GetMouseButtonDown(0))
        {
            ActivateSelectedPhoto();
        }
    }

    public void ActivateSelectedPhoto()
    {

        StartCoroutine(ImageTransition());

    }

    public IEnumerator ImageTransition()
    {

        movingPhotos = true;

        yield return StartCoroutine(activePhoto.ImageTransition());

        if (!activePhoto.IsSelected())
        {

            movingPhotos = false;

        }
    }

    public IEnumerator Rotate(Vector3 rotationDelta, float duration)
    {

        movingPhotos = true;
        Vector3 startRotation = photoHolder.transform.localRotation.eulerAngles;
        Vector3 endRotation = startRotation + rotationDelta;

        Debug.Log("ROTATING BY" + rotationDelta.ToString());

        for (float time = 0; time < duration; time += Time.deltaTime)
        {

            photoHolder.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, time / duration));
            yield return null;

        }

        photoHolder.transform.localRotation = Quaternion.Euler(endRotation);

        movingPhotos = false;

    }

    public IEnumerator Move(Vector3 moveDelta, float duration)
    {

        movingPhotos = true;

        Vector3 startPosition = photoHolder.transform.position;
        Vector3 endPosition = photoHolder.transform.position + moveDelta;

        for (float time = 0; time < duration; time += Time.deltaTime)
        {

            photoHolder.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            yield return null;

        }

        photoHolder.transform.position = endPosition;

        movingPhotos = false;

    }

    public IEnumerator MoveAndRotate(Vector3 moveDelta, Vector3 rotateDelta, float duration)
    {

        if (moveDelta.x != 0 || moveDelta.y != 0 || moveDelta.z != 0)
        {
            yield return StartCoroutine(Move(moveDelta, duration));
        }

        if (rotateDelta.x != 0 || rotateDelta.y != 0 || rotateDelta.z != 0)
        {
            yield return StartCoroutine(Rotate(rotateDelta, duration));
        }
    }
}
