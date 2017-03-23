using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextComponent : POIComponent {

    public string sentence;

    public override void Activate(GameManager gameManager)
    {
        base.Activate(gameManager);

        GameManager.instance.monitorText.text = sentence;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        GameManager.instance.monitorText.text = GameManager.instructionText;

    }
}
