using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class PointDestruct : MonoBehaviour {

  [SerializeField]
  private string filepath;

  private string writePath;
  private string fileExtension = ".off";

  [SerializeField]
  private float pointsToSkip = 5;

  public PointCloudManager pointManager;

	// Use this for initialization
	void Start () {

    writePath = filepath + "Destructed" + pointsToSkip;
    filepath += fileExtension;

    StreamReader sr = new StreamReader(Application.dataPath + filepath);

    int writePathInc = 0;

    if(System.IO.File.Exists(Application.dataPath + writePath + fileExtension)) {

      Debug.LogWarning("Destructed version with point skip value of " + pointsToSkip + " already exists!");

      pointManager.dataPath = (writePath);
      pointManager.gameObject.SetActive(true);
      this.gameObject.SetActive(false);

      return;

    }
     
    StreamWriter sw = new StreamWriter(Application.dataPath + writePath + fileExtension);
    
    //COFF
    sw.WriteLine(sr.ReadLine());

    char[] sep = new char[] { ' ' };
    int lineNum = Convert.ToInt32(sr.ReadLine().Split(sep)[0]);
    string pointInfo = "" + (int)((lineNum/(pointsToSkip+1))) + " 0 0";

    sw.WriteLine(pointInfo);

    while (!sr.EndOfStream) {

      string line = sr.ReadLine();

       sw.WriteLine(line);

      //reads past lines that we don't want to use
      for(int i = 0; i < pointsToSkip; i++) {

        if (!sr.EndOfStream)
          sr.ReadLine();

      }
    }

    sr.Close();
    sw.Close();

    pointManager.dataPath = (writePath);
    pointManager.gameObject.SetActive(true);
    this.gameObject.SetActive(false);


  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
