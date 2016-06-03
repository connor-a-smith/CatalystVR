using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles colorings and activatabilility of buttons.
/// </summary>
public abstract class MonitorButtonScript : MonoBehaviour
{


    protected bool activatable = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Toggle if activatable.
    /// </summary>
    public abstract void AttemptToggle();

    void OnMouseDown()
    {
        if (activatable)
        {
            AttemptToggle();
        }
    }

    public abstract void OnNewNodeSelected();

    public abstract void OnNodeDeselected();

}