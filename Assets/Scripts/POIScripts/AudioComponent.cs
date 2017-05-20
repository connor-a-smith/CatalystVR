using UnityEngine;
using System.Collections;

//Potentially use coroutines for multiple clips if needed?
public class AudioComponent : POIComponent
{
    public AudioClip soundClip;

    public override void Activate(GameManager gameManager, CatalystSite associatedSite)
    {

        base.Activate(gameManager, associatedSite);

        AudioSource source = GameManager.monitor.GetComponentInChildren<AudioSource>();
        source.clip = soundClip;
        source.Play();

    }

    public override void Deactivate()
    {
        base.Deactivate();

        if (GameManager.monitor != null)
        {

            AudioSource source = GameManager.monitor.GetComponentInChildren<AudioSource>();
            source.Stop();
        }
    }
}
