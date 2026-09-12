using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour 
{
    Animator animator;
    PlayerControl playerControl;
    GravityController gravityController;

    int isRunningHash;
    int isTurningHash;

    float previousInput;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isRunningHash = Animator.StringToHash("isRunning");
        isTurningHash = Animator.StringToHash("isTurning");

        playerControl = GetComponentInParent<PlayerControl>();
        gravityController = GetComponentInParent<GravityController>();

        previousInput = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = playerControl.GetMoveInput();

        // Player is NOT on the ground.
        if (!playerControl.IsGrounded())
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isTurningHash, false);

            previousInput = moveInput;

            // Later:
            // Activate jumping/falling animation here.

            return;
        }

        // No input.
        if (moveInput == 0)
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isTurningHash, false);

            previousInput = 0f;

            return;
        }

        // Player has changed direction.
        if (previousInput != 0 && moveInput != previousInput)
        {
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isTurningHash, true);
        }

        // Get the player's actual velocity.
        Vector3 velocity = playerControl.GetVelocity();

        // Get the player's current up direction.
        Vector3 upDirection = gravityController.GetUpDirection();

        // Remove the gravity portion of the velocity.
        Vector3 surfaceVelocity = Vector3.ProjectOnPlane(velocity, upDirection);

        // Get the direction the player is trying to move.
        Vector3 inputDirection = Camera.main.transform.right * moveInput;

        // Remove any gravity component.
        inputDirection =Vector3.ProjectOnPlane(inputDirection, upDirection).normalized;

        // Check whether the player has actually started
        // moving in the direction of the new input.
        float movementDirection = Vector3.Dot(surfaceVelocity.normalized, inputDirection);

        // The player is actually moving in the new direction.
        if (surfaceVelocity.magnitude > 0.05f && movementDirection > 0.1f)
        {
            animator.SetBool(isTurningHash, false);
            animator.SetBool(isRunningHash, true);
        }

        previousInput = moveInput;

    }
}
