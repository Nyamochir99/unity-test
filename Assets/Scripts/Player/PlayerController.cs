using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardspeed;
    public float maxspeed;
    private int desiredlane = 1;
    public float laneDistance = 4;
    public float jumpForce;
    public float Gravity = -20;
    public Animator animator;
    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        if (forwardspeed < maxspeed)
            forwardspeed += 0.1f * Time.deltaTime;

        animator.SetBool("isGameStarted", true);
        direction.z = forwardspeed;

        animator.SetBool("isGrounded", controller.isGrounded);
        if (controller.isGrounded)
        {
            direction.y = -1f;
            if (SwipeManager.swipeUp)
            {
                Jump();
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

        if (SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }

        if (SwipeManager.swipeRight)
        {
            desiredlane++;
            if (desiredlane == 3)
                desiredlane = 2;
        }
        if (SwipeManager.swipeLeft)
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

        Vector3 move = (newPosition - transform.position) + direction * Time.deltaTime;
        controller.Move(move);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindAnyObjectByType<AudioManager>().Playsound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        float originalStepOffset = controller.stepOffset;
        controller.stepOffset = 0;
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        controller.stepOffset = originalStepOffset;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}
