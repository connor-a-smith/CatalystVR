using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoaderComponent : POIComponent {

    public string sceneName;

    public override void Activate(GameManager gameManager)
    {
        base.Activate(gameManager);

        POIManager.selectedPOI.Deactivate();

        SceneManager.LoadSceneAsync(sceneName);

        gameManager.platform.GetComponentInChildren<BookmarkController>().MovePanelDown();

    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
