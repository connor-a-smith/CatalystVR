using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LoadPhotos : MonoBehaviour {

    List<Sprite> sprites;

    void Awake()
    {
        // load all images in sprites array
        sprites = LoadPNG();
    }

    void Start()
    {
        // create the object
        GameObject photo = new GameObject();

        // add a "SpriteRenderer" component to the newly created object
        photo.AddComponent<SpriteRenderer>();

        // assign the first loaded sprite to it
        photo.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }

    /// <summary>
    /// Loads all pngs from the photos directory. Unknown if works with other file types? 
    /// </summary>
    /// <returns></returns>
    public static List<Sprite> LoadPNG()
    {
        //Debug.Log(Directory.GetCurrentDirectory());
        string[] files = Directory.GetFiles("Assets/Resources/Photos");
        List<Sprite> sprites = new List<Sprite>();

        Debug.Log("Number of files: " + files.Length);

        //For each file, load its data and convert to a sprite.
        for (int i = 0; i < files.Length; i++)
        {
            Texture2D tex = null;
            byte[] fileData;

            //Check if file exists and if its a png.
            if (File.Exists(files[i]) && Path.GetExtension(files[i]) == ".png")
            {
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

}
