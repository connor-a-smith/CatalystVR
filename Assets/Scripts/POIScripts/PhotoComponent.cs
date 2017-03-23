using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotoComponent : POIComponent
{

    private bool alreadyLoaded = false;

    [SerializeField]
    private bool pathIsExternal = false;

    [SerializeField]
    private string photoPath;

    [SerializeField]
    private int maximumNumberOfPictures = 24;

    public override void Activate(GameManager gameManager)
    {

        // Calls Activate on parent object.
        base.Activate(gameManager);

        gameManager.photoController.LoadPhotos(photoPath);

    }

    public override void Deactivate()
    {

        base.Deactivate();

        gameManager.photoController.UnloadPhotos();

    }
}
