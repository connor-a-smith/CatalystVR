using UnityEngine;
using System.Collections;

public enum ComponentType { Photos, Audio, Videos, Text, Scene, Zoom, Back }

public abstract class POIComponent : MonoBehaviour
{

    bool activated;
    public bool activateImmediately = true;

    protected GameManager gameManager;

    public virtual void Activate(GameManager gameManager)
    {
        this.gameManager = gameManager;
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
