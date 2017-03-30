using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformInfoText : MonoBehaviour {

    private Text infoText;

    public void SetText(string newText)
    {

        if (infoText == null && this != null)
        {
            infoText = GetComponent<Text>();
        }

        if (infoText != null)
        {
            infoText.text = newText;
        }

    }
}
