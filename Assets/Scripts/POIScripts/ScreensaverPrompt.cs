using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreensaverPrompt : MonoBehaviour {

    public Text ScreensaverText;
    private float y;
 

    // Use this for initialization
    void Start () {

        activateText();

    }
	
	// Update is called once per frame
	void Update () {

        activateText();

    }

    void activateText()
    {
        Debug.Log("Method was called");
        if (GameManager.gameState == GameManager.State.IDLE)
        {
            Debug.Log("Text should show");
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Text should not show");
            gameObject.SetActive(false);
        }

    }
}
