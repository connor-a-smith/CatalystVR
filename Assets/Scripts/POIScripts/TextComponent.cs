using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextComponent : POIScriptComponent {

    public string sentence;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();
        Controller.instance.monitorText.text = sentence;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Controller.instance.monitorText.text = Controller.instructionText;

    }
}
