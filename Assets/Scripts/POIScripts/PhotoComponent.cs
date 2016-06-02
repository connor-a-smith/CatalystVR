using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotoComponent : POIScriptComponent     {

    public string dataPath;

    private GameObject loaderObj;

    // Use this for initialization
    void Start () {
        loaderObj = new GameObject();
        loaderObj.transform.position = this.transform.position;
        loaderObj.transform.parent = this.transform;
        
        loaderObj.AddComponent<LoadPhotos>();
        loaderObj.GetComponent<LoadPhotos>().photoPath = dataPath;
        loaderObj.GetComponent<LoadPhotos>().Load();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();

        loaderObj.SetActive(true);

    }

    public override void Deactivate()
    {
        base.Deactivate();

        loaderObj.SetActive(false);

    }
}
