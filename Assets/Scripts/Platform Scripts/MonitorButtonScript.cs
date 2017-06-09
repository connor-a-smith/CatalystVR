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

    public void Awake()
    {
        
        foreach (MonitorButtonState buttonState in GetComponentsInChildren<MonitorButtonState>(true))
        {

            if (buttonState.state == MonitorButtonState.ButtonState.ACTIVE)
            {
                activeSprite = buttonState.gameObject;
            }
            else if (buttonState.state == MonitorButtonState.ButtonState.DISABLED)
            {
                inactiveSprite = buttonState.gameObject;
            }
            else if (buttonState.state == MonitorButtonState.ButtonState.SELECTED)
            {
                selectedSprite = buttonState.gameObject;
            }
            else if (buttonState.state == MonitorButtonState.ButtonState.PRESSED)
            {
                pressedSprite = buttonState.gameObject;
            }
        }
        
    }

    public void Start()
    {


    }

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

        if (activatable)
        {

            CatalystSite selectedSite = POIManager.selectedPOI.associatedSite;

            if (connectedType == ComponentType.Picture)
            {

                 //selectedSite.ShowImages

            }

            else if (connectedType == ComponentType.Audio)
            {

                //TODO: Add audio to JSON
                

            }

            else if (connectedType == ComponentType.Video)
            {

                // Not implemented yet.

            }

            else if (connectedType == ComponentType.CAVECam)
            {

                selectedSite.StartCoroutine(selectedSite.ShowCAVECams());

            }

            else if (connectedType == ComponentType.Site3D)
            {

                selectedSite.StartCoroutine(selectedSite.Show3DSites());


            }
            else if (connectedType == ComponentType.Artifact)
            {

                // Not implemented yet.

            }
        }
    }

    public void UpdateButtonState()
    {
        if (SiteElementExists())
        {
            showActiveSprite();
            activatable = true;
        }

        else
        {
            showInactiveSprite();
        }
    }

    public bool SiteElementExists()
    {

        CatalystSite selectedSite = POIManager.selectedPOI.associatedSite;

        if (connectedType == ComponentType.Picture)
        {

            return (selectedSite.siteData.images != null);

        }

        else if (connectedType == ComponentType.Audio)
        {

            //TODO: Add audio to JSON
            return false;

        }

        else if (connectedType == ComponentType.Video)
        {

            return (selectedSite.siteData.videos != null);

        }

        else if (connectedType == ComponentType.CAVECam)
        {
            return (selectedSite.siteData.panos != null);

        }

        else if (connectedType == ComponentType.Site3D)
        {

            return (selectedSite.siteData.sites3D != null);


        }
        else if (connectedType == ComponentType.Artifact)
        {
            return (selectedSite.siteData.artifacts != null);
        }

        return false;

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