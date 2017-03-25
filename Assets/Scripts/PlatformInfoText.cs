using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformInfoText : MonoBehaviour {

    private Text infoText;

	// Use this for initialization
	void Start () {

        infoText = GetComponent<Text>();

	}

    public void SetText(string newText)
    {

        infoText.text = newText;

    }
}
