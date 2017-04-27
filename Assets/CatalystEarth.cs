using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CatalystEarth : MonoBehaviour {

    private float planetRadius;

    public struct LatLon
    {

        float lat;
        float lon;

        public LatLon(float lat, float lon)
        {
            this.lat = lat;
            this.lon = lon;
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CalculateRadius()
    {

        planetRadius = GetComponent<SphereCollider>().radius;

    }

    public static Vector3 Get3DPositionFromLatLon(float latitude, float longitude)
    {

        return Get3DPositionFromLatLon(new LatLon(latitude, longitude));


    }

    public static Vector3 Get3DPositionFromLatLon(LatLon latLon)
    {

        return Vector3.zero;


    }

    public static LatLon GetLatLongFromVector3(Vector3 position, float sphereRadius)
    {
        float lat = (float)Mathf.Acos(position.y / sphereRadius); //theta
        float lon = (float)Mathf.Atan(position.x / position.z); //phi
        return new LatLon(lat, lon);
    }
}
