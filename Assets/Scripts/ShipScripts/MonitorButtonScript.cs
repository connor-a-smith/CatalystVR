using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles colorings and activatabilility of buttons.
/// </summary>
public class MonitorButtonScript : MonoBehaviour
{
    public ComponentType connectedType;
    private POIScriptComponent connectedComponent;

    public GameObject activeSprite;
    public GameObject inactiveSprite;
    public GameObject selectedSprite;
    public GameObject pressedSprite;

    protected bool activatable = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (activatable)
        {
            AttemptToggle();
        }
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

    public void OnNewNodeSelected()
    {
        GetComponentFromNode();

        if (connectedComponent)
        {
            activeSprite.SetActive(true);
            inactiveSprite.SetActive(false);
            activatable = true;
        }

        else
        {
            activeSprite.SetActive(false);
            inactiveSprite.SetActive(true);
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

        else if (connectedType == ComponentType.Scene)
        {
            connectedComponent = Controller.selectedPOI.GetComponent<SceneLoaderComponent>();

        }

        else if (connectedType == ComponentType.Zoom)
        {
            connectedComponent = Controller.selectedPOI.GetComponent<FocusTransformComponent>();

        }

        else
        {
            Debug.LogError("Component not setup in MonitorButtonScript");
        }
    }
}