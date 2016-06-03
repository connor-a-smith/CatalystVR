using UnityEngine;
using System.Collections;

public class ForwardScript : MonoBehaviour {

  public float speed = 2.0f;

	// Use this for initialization
	void Start () {
   // Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
	  if (Input.GetMouseButton(0)) {
      transform.position += transform.GetChild(0).forward * speed;
    }

    if(Input.GetMouseButton(1)) {
      transform.position -= transform.GetChild(0).forward * speed;
    }
    //Cursor.lockState = CursorLockMode.Locked;


  }
}
