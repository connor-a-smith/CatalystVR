using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreensaverText : MonoBehaviour {

    //public Text myScreensaverText;
    private float y;
    public ScreensaverPlane myPlane;
    public Transform target;
    
 

    // Use this for initialization
    void Start () {

      //  activateText();

    }
	
	// Update is called once per frame
	void Update () {

      //  activateText();

    }

    public void changePosition()
    {    
        gameObject.transform.localPosition = new Vector3(0, 0, 200);
        float planeX = Random.Range(-100.0f, 100.0f);
        float planeY = Random.Range(-40.0f, 40.0f);
        float planeZ = gameObject.transform.localPosition.z;
        gameObject.transform.localPosition = new Vector3(planeX, planeY, planeZ);
        target = GameManager.instance.cameraRig.viewpoint.transform;
        gameObject.transform.LookAt(target);
        gameObject.transform.localPosition = new Vector3(planeX, planeY, planeZ);
    }

    void activateText()
    {
        Debug.Log("Method was called");
        if (GameManager.gameState == GameManager.State.IDLE)
        {
            Debug.Log("Text should show");
            gameObject.SetActive(true);
            //myPlane.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Text should not show");
            gameObject.SetActive(false);
            //myPlane.gameObject.SetActive(false);
        }

    }
}
