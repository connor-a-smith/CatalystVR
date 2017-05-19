﻿using UnityEngine;
using System.Collections;

public enum ComponentType { Photos, Audio, Videos, Text, Scene, Zoom, Back }

public abstract class POIComponent : MonoBehaviour
{

    bool activated;
    public bool activateImmediately = true;

    protected GameManager gameManager;

    private CatalystSite associatedSite;

    public virtual void Activate(GameManager gameManager, CatalystSite associatedSite)
    {
        this.gameManager = gameManager;
        this.associatedSite = associatedSite;
        activated = true;
    }

    public virtual void Deactivate()
    {
        activated = false;
    }

    /// <summary>
    /// Toggles the current state.
    /// </summary>
    public void Toggle(GameManager gameManager)
    {
        if (!activated)
        {
            Activate(gameManager);
        }

        else
        {
            Deactivate();
        }
    }
}
