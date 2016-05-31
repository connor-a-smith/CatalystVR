using UnityEngine;
using System.Collections;

//Potentially use coroutines for multiple clips if needed?
public class AudioComponent : POIScriptComponent
{
    public AudioClip soundClip;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate()
    {
        base.Activate();

        AudioSource source = Controller.monitor.GetComponentInChildren<AudioSource>();
        source.clip= soundClip;
        source.Play();

    }

    public override void Deactivate()
    {
        base.Activate();
        AudioSource source = Controller.monitor.GetComponentInChildren<AudioSource>();
        source.Stop();
    }
}
