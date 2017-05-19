using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour {

    [SerializeField] private string jsonDataPath = GameManager.dataDirectory + "/siteData.json";

	// Use this for initialization
	void Start () {

        CatalystSiteData tmp = new CatalystSiteData();
        File.WriteAllText(jsonDataPath, JsonUtility.ToJson(tmp));

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class CatalystSiteData
{

    SerializableCatalystSite[] sites;

}
