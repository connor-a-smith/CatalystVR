using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SiteManager : MonoBehaviour {

    public List<CatalystSite> sites;
    private GameManager gameManager;

    public void LoadSites(string dataPath)
    {
        gameManager = GetComponentInParent<GameManager>();

        if (!File.Exists(GameManager.dataJsonFile))
        {

            SerializableCatalystSites sampleSites = new SerializableCatalystSites();

            sampleSites.sites = new CatalystSite[1];

            CatalystSite newSite = new CatalystSite();

            // UC San Diego Lat/Lon as sample
            newSite.latitude = 32.8801f;
            newSite.longitude = 117.2340f;
            newSite.name = "UC San Diego";
            newSite.description = "This site was generated as a sample of what the JSON file should look like, roughly";

            sampleSites.sites[0] = newSite;

            string jsonText = JsonUtility.ToJson(sampleSites);

            File.WriteAllText(GameManager.dataJsonFile, jsonText);

            return;

        }

        string jsonString = File.ReadAllText(GameManager.dataJsonFile);

        SerializableCatalystSites siteData = JsonUtility.FromJson<SerializableCatalystSites>(jsonString);

        if (siteData.sites != null && siteData.sites.Length > 0)
        {
            sites = new List<CatalystSite>(siteData.sites);
        }
        else
        {
            Debug.LogErrorFormat("Error: No sites loaded. Please check the following file: {0}", GameManager.dataJsonFile);
            return;
        }

        Debug.LogFormat("Loaded {0} sites", sites.Count);

        POIManager poiManager = gameManager.GetComponentInChildren<POIManager>();

        foreach (CatalystSite site in sites)
        {

            poiManager.CreateNewPOI(site);

        }
    }

        // Use this for initialization
    void Start () {

        LoadSites(GameManager.dataJsonFile);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [System.Serializable]
    private class SerializableCatalystSites
    {

        public CatalystSite[] sites;

    }
}


