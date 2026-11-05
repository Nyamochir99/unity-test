using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontrol : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardspeed;
    private int desiredlane = 1;
    public float laneDistance = 4;
    public float jumpForce;
    public float Gravity = -20;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        direction.z = forwardspeed;
        direction.y += Gravity * Time.deltaTime;

        if (controller.isGrounded)
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                Jump();
            }
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            desiredlane++;
            if (desiredlane == 3)
                desiredlane = 2;
        }
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            desiredlane--;
            if (desiredlane == -1)
                desiredlane = 0;
        }

        Vector3 targetPosition =
            transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredlane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredlane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, 80 * Time.deltaTime);
        controller.Move(newPosition - transform.position);
    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }
}
