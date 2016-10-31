using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ControllerInput : MonoBehaviour
{
    public float xRotationSpeed = 30;
    public float yRotationSpeed = -30;

    public float forwardSpeed = 5;
    private bool justActivated = false;

    public Camera raycastCam;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("RightStickHorizontal") != 0)
        {
            //Controller.playerShip.transform.Rotate(0, Time.deltaTime * Input.GetAxis("RightStickHorizontal") * xRotationSpeed, 0, Space.Self, );
            Controller.playerShip.transform.RotateAround(raycastCam.transform.parent.position, Vector3.up, Time.deltaTime * Input.GetAxis("RightStickHorizontal") * xRotationSpeed);

        }

        if (Input.GetAxis("RightStickVertical") != 0)
        {
            Controller.playerShip.transform.Rotate(Time.deltaTime * Input.GetAxis("RightStickVertical") * yRotationSpeed, 0, 0, Space.Self);
            //Controller.playerShip.transform.RotateAround(raycastCam.transform.parent.transform.position, Vector3.left, Time.deltaTime * Input.GetAxis("RightStickVertical") * yRotationSpeed);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            //Debug.Log(Input.GetAxis("RightStickHorizontal"));
            Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("Horizontal") * forwardSpeed * Controller.playerShip.transform.right);
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            //Debug.Log(Input.GetAxis("RightStickVertical"));
            Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("Vertical") * forwardSpeed * Controller.playerShip.transform.forward);
        }

        //On trigger press, activate
        if (Input.GetAxis("RightTrigger") != 0 && !justActivated)
        {
            //Debug.Log(Input.GetAxis("RightTrigger"));
            justActivated = true;

            RaycastHit hit;
            Physics.Raycast(raycastCam.transform.position, raycastCam.transform.forward,out hit, Mathf.Infinity);
            
            Controller.inputManager.HandleHit(hit);

        }

        else if (Input.GetAxis("RightTrigger") == 0)
        {
            justActivated = false;
        }

        if (Input.GetKey(KeyCode.A))//else if (Input.GetButton("joystick button 0"))
        {
            SceneManager.LoadScene("Mar Saba");
        }
        if (Input.GetKey(KeyCode.S))//else if (Input.GetButton("joystick button 0"))
        {
            SceneManager.LoadScene("Luxor");
        }
        if (Input.GetKey(KeyCode.D))//else if (Input.GetButton("joystick button 0"))
        {
            SceneManager.LoadScene("MultiDisplayPlanet");
        }
    }


}
