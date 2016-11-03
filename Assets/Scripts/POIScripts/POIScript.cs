using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class POIScript : MonoBehaviour {

  bool activated = false;
  List<POIScriptComponent> components;

  // Use this for initialization
  void Start() {
    components = new List<POIScriptComponent>(GetComponentsInChildren<POIScriptComponent>());
    Controller.instance.POIList.Add(this);
  }

  // Update is called once per frame
  void Update() {
    //No need to look at player anymore. btw messes up child objects too, such as focus positions.
    // transform.LookAt(Controller.playerShip.transform.position);
  }

  void OnMouseDown() {
    Toggle();
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

    