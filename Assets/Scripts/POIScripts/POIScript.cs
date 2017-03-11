using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class POIScript : MonoBehaviour
{

    bool activated = false;
    public List<POIScriptComponent> components;
    public string POIName;

    [SerializeField] private float labelSpawnHeight = 1.5f;

    private void Awake()
    {

        POIManager.AddPOI(this);

    }

    public void OnDestroy()
    {

        Deactivate();

    }


    void OnMouseDown()
    {
        Toggle();
    }

    // Use this for initialization
    void Start()
    {

        components = new List<POIScriptComponent>(GetComponentsInChildren<POIScriptComponent>());
        GameObject label = GameObject.Instantiate(GameManager.instance.labelPrefab, this.transform, false) as GameObject;
        Vector3 newPosition = label.transform.localPosition;
        newPosition.y += labelSpawnHeight;
        label.transform.localPosition = newPosition;
        label.GetComponentInChildren<Text>().text = POIName;
    }

    public void Activate()
    {
        //If selected another node without deactivating an old one, then deactivate the old one.
        if (POIManager.selectedPOI != null && POIManager.selectedPOI != this && POIManager.selectedPOI)
        {
            POIManager.selectedPOI.Deactivate();
        }

        activated = true;
        POIManager.selectedPOI = this;
        GetComponentInChildren<Renderer>().material = POIManager.selectedPOIMat;

        //Tell all the buttons that a new poi was selected.
        for (int i = 0; i < GameManager.buttons.Length; i++)
        {
            GameManager.buttons[i].gameObject.SetActive(true);

            GameManager.buttons[i].OnNewNodeSelected();
        }

        //Tell all components to activate.
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i].activateImmediately)
            {
                components[i].Activate();
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


        //Tell all the buttons that there is no current POI.
        for (int i = 0; i < GameManager.buttons.Length; i++)
        {
            GameManager.buttons[i].OnNodeDeselected();
            GameManager.buttons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < components.Count; i++)
        {
            components[i].Deactivate();
        }
    }

    /// <summary>
    /// A way to toggle the state without needing to check the current state externally.
    /// </summary>
    public void Toggle()
    {
        if (!activated)
        {
            Activate();
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

