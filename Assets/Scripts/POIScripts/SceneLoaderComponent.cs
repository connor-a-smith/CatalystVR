using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoaderComponent : POIComponent {

    public override void Activate(GameManager gameManager, CatalystSite associatedSite)
    {
        base.Activate(gameManager, associatedSite);

        POIManager.selectedPOI.Deactivate();

        StartCoroutine(associatedSite.ShowCAVECams());

        gameManager.platform.GetComponentInChildren<BookmarkController>().MovePanelDown();

    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
