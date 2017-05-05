[System.Serializable]
public class CatalystSite
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
