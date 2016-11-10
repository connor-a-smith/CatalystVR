using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class POIScript : MonoBehaviour {

  bool activated = false;
  public List<POIScriptComponent> components;
  public string POIName;
  [SerializeField]
  private float labelSpawnHeight = 1.5f;

  void Awake() {

    Controller.controllerReady += AddPOI;

  }

  public void AddPOI() {

    Controller.instance.POIList.Add(this);

  }

  public void OnDestroy() {

    Controller.controllerReady -= AddPOI;

  }


  void OnMouseDown() {
    Toggle();
  }

  // Use this for initialization
  void Start() {
    components = new List<POIScriptComponent>(GetComponentsInChildren<POIScriptComponent>());
    GameObject label = GameObject.Instantiate(Controller.instance.labelPrefab, this.transform, false) as GameObject;
    Vector3 newPosition = label.transform.localPosition;
    newPosition.y += labelSpawnHeight;
    label.transform.localPosition = newPosition;
    label.GetComponentInChildren<Text>().text = POIName;

    SceneManager.sceneUnloaded += DeactivateOnSceneUnload;
  }

  public void OnDestroy() {

    SceneManager.sceneUnloaded -= DeactivateOnSceneUnload;

  }

  // Update is called once per frame
  void Update() {
    //No need to look at player anymore. btw messes up child objects too, such as focus positions.
    // transform.LookAt(Controller.playerShip.transform.position);
  }

  public void Activate() {
    //If selected another node without deactivating an old one, then deactivate the old one.
    if (Controller.selectedPOI != null && Controller.selectedPOI != this && Controller.selectedPOI) {
      Controller.selectedPOI.Deactivate();
    }

    activated = true;
    Controller.selectedPOI = this;
    GetComponentInChildren<Renderer>().material = Controller.selectedPOIMat;

    //Tell all the buttons that a new poi was selected.
    for (int i = 0; i < Controller.buttons.Length; i++) {
      Controller.buttons[i].gameObject.SetActive(true);

      Controller.buttons[i].OnNewNodeSelected();
    }

    //Tell all components to activate.
    for (int i = 0; i < components.Count; i++) {
      if (components[i].activateImmediately) {
        components[i].Activate();
      }
    }
  }

  public void DeactivateOnSceneUnload(Scene scene) {

    Deactivate();

  }

  public void Deactivate() {
    activated = false;
    Controller.selectedPOI = null;
    GetComponentInChildren<Renderer>().material = Controller.defaultPOIMat;


    //Tell all the buttons that there is no current POI.
    for (int i = 0; i < Controller.buttons.Length; i++) {
      Controller.buttons[i].OnNodeDeselected();
      Controller.buttons[i].gameObject.SetActive(false);
    }

    for (int i = 0; i < components.Count; i++) {
      components[i].Deactivate();
    }
  }

  /// <summary>
  /// A way to toggle the state without needing to check the current state externally.
  /// </summary>
  public void Toggle() {
    if (!activated) {
      Activate();
    }
    else {
      Deactivate();
    }
  }

  /// <summary>
  /// Highlight the POI if it is hit in the raycast.
  /// </summary>
  public void Highlight() {
    GetComponentInChildren<Renderer>().material = Controller.highlightedPOIMat;
  }

  /// <summary>
  /// UnHighlight the POI if it is not hit in the raycast.
  /// </summary>
  public void UnHighlight() {
    GetComponentInChildren<Renderer>().material = Controller.defaultPOIMat;
  }
}

