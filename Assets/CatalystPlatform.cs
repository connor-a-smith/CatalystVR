using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystPlatform : MonoBehaviour
{

    // Modifiers for speed and max/min tilt angles.
    [SerializeField] private float xRotationSpeed = 30;
    [SerializeField] private float yRotationSpeed = -30;
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float maxTiltAngle = 45.0f;

    // Input options for different movement.
    private GamepadInput.InputOption horizontalRotationInput = GamepadInput.InputOption.RIGHT_STICK_HORIZONTAL;
    private GamepadInput.InputOption verticalRotationInput = GamepadInput.InputOption.RIGHT_STICK_VERTICAL;
    private GamepadInput.InputOption horizontalMovementInput = GamepadInput.InputOption.LEFT_STICK_HORIZONTAL;
    private GamepadInput.InputOption forwardMovementInput = GamepadInput.InputOption.LEFT_STICK_VERTICAL;

    // Update is called once per frame
    void Update()
    {
        
        // The platform should only move if no POI is selected and the bookmarks aren't being used.
        if (GameManager.selectedPOI == null && !GameManager.instance.bookmarks.bookmarkPanelActivated)
        {

            // Rotates the platform horizontally.
            if (GamepadInput.Get(horizontalRotationInput))
            {

                RotatePlatformHorizontally(GamepadInput.GetInputValue(horizontalRotationInput));

            }

            // Rotates the platform vertically.
            if (GamepadInput.Get(verticalRotationInput))
            {

                RotatePlatformVertically(GamepadInput.GetInputValue(verticalRotationInput));

            }

            // Moves the platform horizontally.
            if (GamepadInput.Get(horizontalMovementInput))
            {

                MovePlatform(GamepadInput.GetInputValue(horizontalMovementInput), transform.right);

            }

            // Moves the platform forward.
            if (GamepadInput.Get(forwardMovementInput))
            {

                MovePlatform(GamepadInput.GetInputValue(forwardMovementInput), transform.forward);

            }
        }
    }

    // Moves the platform in a specified direction, based on an input value.
    private void MovePlatform(float inputAxisValue, Vector3 direction)
    {

        transform.position += (Time.deltaTime * inputAxisValue * movementSpeed * direction);

    }

    // Rotates the platform horizontally, based on an input value.
    private void RotatePlatformHorizontally(float inputAxisValue)
    {

        float rotateAngle = Time.deltaTime * inputAxisValue * xRotationSpeed;

        this.transform.RotateAround(transform.position, Vector3.up, rotateAngle);

    }

    // Rotates the platform vertically, with bounds to prevent full rotation.
    private void RotatePlatformVertically(float inputAxisValue)
    {

        float rotateAngle = Time.deltaTime * inputAxisValue * yRotationSpeed;

        // Rotates the platform initially.
        transform.Rotate(rotateAngle, 0, 0, Space.Self);

        // Stores the rotation of the platform, as euler angles because Quaternions are complex.
        Vector3 platformRotation = transform.localRotation.eulerAngles;

        // Keep the rotations between -180 and 180. Easier to think about.
        if (platformRotation.x > 180.0f)
        {       
            platformRotation.x -= 360.0f;
        }

        // If the user attempted to tilt below the min tilt angle, snap to the min.
        if (platformRotation.x < -maxTiltAngle)
        {
            platformRotation.x = -maxTiltAngle;
        }

        // If the user attempted to tilt above the max tilt angle, snap to the max.
        else if (platformRotation.x > maxTiltAngle)
        {
            platformRotation.x = maxTiltAngle;
        }

        // Z rotation should always be 0, regardless of rotations in other axes.
        platformRotation.z = 0.0f;

        // Updates the rotation of the platform with the correct snapped rotation.
        transform.localRotation = Quaternion.Euler(platformRotation);

    }
}
