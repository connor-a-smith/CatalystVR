using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class POI : MonoBehaviour
{

    bool activated = false;

    public List<POIComponent> components;

    public string POIName;

    [SerializeField] private float labelSpawnHeight = 1.5f;

    private void Awake()
    {

    //    POIManager.AddPOI(this);

    }

    public void OnDestroy()
    {

        Deactivate();

    }

    // Use this for initialization
    void Start()
    {

        components = new List<POIComponent>(GetComponentsInChildren<POIComponent>());
        GameObject label = GameObject.Instantiate(GameManager.instance.labelPrefab, this.transform, false) as GameObject;
        Vector3 newPosition = label.transform.localPosition;
        newPosition.z -= labelSpawnHeight;
        label.transform.localPosition = newPosition;
        label.GetComponentInChildren<Text>().text = POIName;
    }

    public void Activate(GameManager gameManager)
    {
        //If selected another node without deactivating an old one, then deactivate the old one.
        if (POIManager.selectedPOI != null && POIManager.selectedPOI != this)
        {
            POIManager.selectedPOI.Deactivate();
        }

        activated = true;
        POIManager.selectedPOI = this;
        GetComponentInChildren<Renderer>().material = POIManager.selectedPOIMat;

        PlatformMonitor.ActivateMonitorButtons();

        // Tell all components to activate.
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i].activateImmediately)
            {
                components[i].Activate(gameManager);
            }
        }
    }

    public void DeactivateOnSceneUnload(Scene scene)
    {

        Deactivate();
        SceneManager.sceneUnloaded -= DeactivateOnSceneUnload;

    }

    public void Deactivate()
    {

        activated = false;
        POIManager.selectedPOI = null;
        GetComponentInChildren<Renderer>().material = POIManager.defaultPOIMat;

        PlatformMonitor.DeactivateMonitorButtons();

        for (int i = 0; i < components.Count; i++)
        {
            components[i].Deactivate();
        }
    }

    /// <summary>
    /// A way to toggle the state without needing to check the current state externally.
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

    /// <summary>
    /// Highlight the POI if it is hit in the raycast.
    /// </summary>
    public void Highlight()
    {
        GetComponentInChildren<Renderer>().material = POIManager.highlightedPOIMat;
    }

    /// <summary>
    /// UnHighlight the POI if it is not hit in the raycast.
    /// </summary>
    public void UnHighlight()
    {
        GetComponentInChildren<Renderer>().material = POIManager.defaultPOIMat;
    }
}

