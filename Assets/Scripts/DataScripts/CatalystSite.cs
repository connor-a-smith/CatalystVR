using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystSite : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}

[System.Serializable]
public class SerializableSite
{

    public string name;
    public string description;
    public float latitude;
    public float longitude;

    SerializableCAVECam[] caveCams;
    SerializableVideo[] videos;
    SerializableModel[] models;
    SerializableImage[] images;
    SerializablePointCloud[] pointClouds;

}
