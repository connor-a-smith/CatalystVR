using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoaderComponent : POIScriptComponent {

    public string sceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public override void Activate()
    {
        base.Activate();
        POIManager.selectedPOI.Deactivate();

        SceneManager.LoadSceneAsync(sceneName);
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
