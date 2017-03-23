using UnityEngine;
using System.Collections;

//Potentially use coroutines for multiple clips if needed?
public class AudioComponent : POIComponent
{
    public AudioClip soundClip;

    public override void Activate(GameManager gameManager)
    {

        base.Activate(gameManager);

        AudioSource source = GameManager.monitor.GetComponentInChildren<AudioSource>();
        source.clip = soundClip;
        source.Play();

    }

    public override void Deactivate()
    {
        base.Deactivate();

        AudioSource source = GameManager.monitor.GetComponentInChildren<AudioSource>();
        source.Stop();
    }
}
