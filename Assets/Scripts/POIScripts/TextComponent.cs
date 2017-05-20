using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextComponent : POIComponent {

    public string sentence;

    public override void Activate(GameManager gameManager, CatalystSite associatedSite)
    {
        base.Activate(gameManager, associatedSite);

        PlatformMonitor.SetMonitorText(sentence);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        PlatformMonitor.ResetMonitorText();

    }
}
