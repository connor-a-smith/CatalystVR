using UnityEngine;
using System.Collections;

public class ControllerInput : MonoBehaviour
{
    public float xRotationSpeed = 30;
    public float yRotationSpeed = -30;

    public float forwardSpeed = 5;
    private bool justActivated = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            Controller.playerShip.transform.Rotate(0, Time.deltaTime * Input.GetAxis("Horizontal") * xRotationSpeed, 0);
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            Controller.playerShip.transform.Rotate(Time.deltaTime * Input.GetAxis("Vertical") * yRotationSpeed, 0, 0);
        }

        if (Input.GetAxis("RightStickHorizontal") != 0)
        {
            //Debug.Log(Input.GetAxis("RightStickHorizontal"));
            Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("RightStickHorizontal") * forwardSpeed * Controller.playerShip.transform.right);
        }

        if (Input.GetAxis("RightStickVertical") != 0)
        {
            //Debug.Log(Input.GetAxis("RightStickVertical"));
            Controller.playerShip.transform.position += (Time.deltaTime * Input.GetAxis("RightStickVertical") * forwardSpeed * Controller.playerShip.transform.forward);
        }

        //On trigger press, activate
        if (Input.GetAxis("RightTrigger") != 0 && !justActivated)
        {
            //Debug.Log(Input.GetAxis("RightTrigger"));
            justActivated = true;

            RaycastHit hit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out hit, Mathf.Infinity);
            
            Controller.inputManager.HandleHit(hit);

        }

        else if (Input.GetAxis("RightTrigger") == 0)
        {
            justActivated = false;
        }


    }


}
