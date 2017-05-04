using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CatalystEarth : MonoBehaviour {

    private static float planetRadius;
    public static Transform earthTransform;

    public struct LatLon
    {

        public float lat;
        public float lon;

        public LatLon(float lat, float lon)
        {
            this.lat = lat;
            this.lon = lon;
        }

    }

    private void Awake()
    {

        CalculateRadius();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CalculateRadius()
    {

        planetRadius = GetComponent<SphereCollider>().radius * 5.32f;
        earthTransform = this.transform;

        Debug.Log("Found radius: " + planetRadius);

    }

    public static Vector3 Get3DPositionFromLatLon(float latitude, float longitude)
    {

        return Get3DPositionFromLatLon(new LatLon(latitude, longitude));


    }

    public static Vector3 Get3DPositionFromLatLon(LatLon latLon)
    {

        float lat = latLon.lat;
        float lon = latLon.lon;

        /*
        float flattening = 0.0f;

        float ls = Mathf.Atan(Mathf.Pow((1 - flattening), 2) * Mathf.Tan(lat));

        float altitude = planetRadius;

        float x = planetRadius * Mathf.Cos(ls) * Mathf.Cos(lon) + altitude * Mathf.Cos(lat) * Mathf.Cos(lon);

        Debug.Log("Radius is still: " + planetRadius);
        Debug.Log("Found X: " + x);

        float y = planetRadius * Mathf.Cos(ls) * Mathf.Sin(lon) + altitude * Mathf.Cos(lat) * Mathf.Sin(lon);

        float z = planetRadius * Mathf.Sin(ls) + altitude * Mathf.Sin(lat);

        Vector3 posRelativeToEarth = new Vector3(x, y, z);

        Debug.LogFormat("Found position relative to earth: ({0}, {1}, {2})", posRelativeToEarth.x, posRelativeToEarth.y, posRelativeToEarth.z);

        Vector3 worldPos = earthCenter + posRelativeToEarth;

        return worldPos;
        */

        Vector3 worldPos = Quaternion.AngleAxis(lon, -Vector3.up) * 
                           Quaternion.AngleAxis(lat, -Vector3.right) * 
                           new Vector3(0.0f, 0.0f, planetRadius);

        worldPos += earthTransform.position;

        return worldPos;

}

    public static LatLon GetLatLongFromVector3(Vector3 position)
    {
        float lat = (float)Mathf.Acos(position.y / planetRadius); //theta
        float lon = (float)Mathf.Atan(position.x / position.z); //phi
        return new LatLon(lat, lon);
    }
}
