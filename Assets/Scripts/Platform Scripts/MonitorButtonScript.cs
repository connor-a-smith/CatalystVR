using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles colorings and activatabilility of buttons.
/// </summary>
public class MonitorButtonScript : MonoBehaviour
{
    public ComponentType connectedType;
    private POIComponent connectedComponent;

    public GameObject activeSprite;
    public GameObject inactiveSprite;
    public GameObject selectedSprite;
    public GameObject pressedSprite;

    public bool activatable = false;


    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float changeTime = 1.0f;

    /// <summary>
    /// Handle what happens when a button is selected.
    /// </summary>
    public void select()
    {
        showSelectedSprite();
    }

    /// <summary>
    /// Handle what happens when a button is deselected.
    /// </summary>
    public void deselect()
    {
        if (activatable)
        {
            showActiveSprite();
        }

        else
        {
            showInactiveSprite();
        }
    }

    void showSelectedSprite()
    {
        activeSprite.SetActive(false);
        inactiveSprite.SetActive(false);
        selectedSprite.SetActive(true);
        pressedSprite.SetActive(false);
    }

    void showActiveSprite()
    {
        activeSprite.SetActive(true);
        inactiveSprite.SetActive(false);
        selectedSprite.SetActive(false);
        pressedSprite.SetActive(false);
    }

    void showInactiveSprite()
    {
        activeSprite.SetActive(false);
        inactiveSprite.SetActive(true);
        selectedSprite.SetActive(false);
        pressedSprite.SetActive(false);
    }

    void showPressedSprite()
    {
        activeSprite.SetActive(false);
        inactiveSprite.SetActive(false);
        selectedSprite.SetActive(false);
        pressedSprite.SetActive(true);
    }
    /// <summary>
    /// Toggle if activatable.
    /// </summary>
    public void ToggleButton()
    {

        if (connectedType == ComponentType.Back)
        {
            POIManager.selectedPOI.Deactivate();
        }

        else if (activatable)
        {
            connectedComponent.Toggle(GetComponentInParent<CatalystPlatform>().gameManager);
        }
    }

    public void UpdateButtonState()
    {
        GetComponentFromNode();

        if (connectedComponent || connectedType == ComponentType.Back)
        {
            showActiveSprite();
            activatable = true;
        }

        else
        {
            showInactiveSprite();
        }
    }

    /// <summary>
    /// Using the enum, determine the connected component 
    /// </summary>
    void GetComponentFromNode()
    {
        if (connectedType == ComponentType.Photos)
        {
            connectedComponent = POIManager.selectedPOI.GetComponent<PhotoComponent>();
        }

        else if (connectedType == ComponentType.Audio)
        {
            connectedComponent = POIManager.selectedPOI.GetComponent<AudioComponent>();

        }

        else if (connectedType == ComponentType.Videos)
        {
            connectedComponent = POIManager.selectedPOI.GetComponent<VideoComponent>();

        }

        else if (connectedType == ComponentType.Text)
        {
            connectedComponent = POIManager.selectedPOI.GetComponent<TextComponent>();

        }

        else if (connectedType == ComponentType.Scene)
        {
            connectedComponent = POIManager.selectedPOI.GetComponent<SceneLoaderComponent>();

        }

        else if (connectedType == ComponentType.Zoom)
        {
            connectedComponent = POIManager.selectedPOI.GetComponent<FocusTransformComponent>();

        }

        else if (connectedType != ComponentType.Back)
        {
            Debug.LogError("Component not setup in MonitorButtonScript");
        }
    }

    IEnumerator selectChangeColor()
    {
        for (float i = 0; i < changeTime; i += Time.deltaTime)
        {
            showPressedSprite();
            yield return null;
        }

        showSelectedSprite();
    }
}