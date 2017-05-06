//Using http://forum.unity3d.com/threads/dynamic-load-play-movie-texture.45043/
//Only works with ogv files.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class LoadVideos : MonoBehaviour
{
    MovieTexture movieTexture;

    void Start()
    {
        WWW www = new WWW("http://www.unity3d.com/webplayers/Movie/sample.ogg");
        movieTexture = (MovieTexture)www.GetMovieTexture();
        this.GetComponent<Renderer>().material.SetTexture("_MainTex", movieTexture);
        GetComponent<AudioSource>().clip = movieTexture.audioClip;
    }

    void Update()
    {
        if (movieTexture.isReadyToPlay && !movieTexture.isPlaying)
        {
            movieTexture.Play();
            GetComponent<AudioSource>().Play();
        }
    }

/*    /// <summary>
    /// Loads all ogv videos from the Videos directory. Unknown if works with other file types? 
    /// </summary>
    /// <returns></returns>
    public static List<Sprite> LoadVideoFiles()
    {
        //Debug.Log(Directory.GetCurrentDirectory());
        string[] files = Directory.GetFiles("Assets/Resources/Videos");
        List<Sprite> sprites = new List<Sprite>();

        Debug.Log("Number of files: " + files.Length);

        //For each file, load its data and convert to a sprite.
        for (int i = 0; i < files.Length; i++)
        {
            //Check if file exists and if its a png.
            if (File.Exists(files[i]) && Path.GetExtension(files[i]) == ".ogv")
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
    }*/

}