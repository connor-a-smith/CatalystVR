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

    public override void Activate(GameManager gameManager, CatalystSite associatedSite)
    {

        // Calls Activate on parent object.
        base.Activate(gameManager, associatedSite);

        gameManager.photoController.LoadPhotos(photoPath);

    }

    public override void Deactivate()
    {

        base.Deactivate();

        if (gameManager != null)
        {
            gameManager.photoController.UnloadPhotos();
        }

    }
}
