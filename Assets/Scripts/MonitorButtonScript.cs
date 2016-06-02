using UnityEngine;
using System.Collections;

public class MonitorButtonScript : MonoBehaviour {
    public ComponentType connectedType;
    private POIScriptComponent connectedComponent;

    private bool activatable = false;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Toggle if activatable.
    /// </summary>
    public void AttemptToggle()
    {
        if (activatable)
        {
            connectedComponent.Toggle();
        }
    }

    void OnMouseDown()
    {
        if (activatable)
        {
            AttemptToggle();
        }
    }

    public void OnNewNodeSelected()
    {
        GetComponentFromNode();

        if (connectedComponent)
        {
            activatable = true;
        }
    }

    public void OnNodeDeselected()
    {
        activatable = false;
    }

    /// <summary>
    /// Using the enum, determine the connected component 
    /// </summary>
    void GetComponentFromNode()
    {
        if (connectedType == ComponentType.Photos)
        {
            connectedComponent = Controller.selectedPOI.GetComponent<PhotoComponent>();
        }

        else if (connectedType == ComponentType.Audio)
        {
            connectedComponent = Controller.selectedPOI.GetComponent<AudioComponent>();

        }

        else if (connectedType == ComponentType.Videos)
        {
            connectedComponent = Controller.selectedPOI.GetComponent<VideoComponent>();

        }

        else if (connectedType == ComponentType.Text)
        {
            connectedComponent = Controller.selectedPOI.GetComponent<TextComponent>();

        }

        else //if (connectedType == ComponentType.Scene)
        {
            connectedComponent = Controller.selectedPOI.GetComponent<SceneLoaderComponent>();

        }
    } 
}
