using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour // DELETE LATER: When I'm moving to the left or right, the model moves as normal, but when I switch direction, the model slows, stops, then moves in the direction I want. It is quite fast, but can I use those moments to change and set an animation so it looks like it was doing a 180-degree turn, and the moment the player starts moving in the opposite direction, the model switches the direction it is facing and goes back to the isrunning phase?
{
    Animator animator;
    PlayerControl playerControl;

    int isRunningHash;

    bool wasGrounded;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        playerControl = GetComponentInParent<PlayerControl>(); // Reminder because the script is in the lower hierarchy.
        wasGrounded = playerControl.IsGrounded();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = playerControl.GetMoveInput();
        bool isGrounded = playerControl.IsGrounded();

        // Player is airborne.
        if (!isGrounded)               // It midigates the problem but the "isRunning" is set on briefly while changing. 
        {
            animator.SetBool(isRunningHash, false);

            wasGrounded = false;

            // Later:
            // Jumping / Falling animation goes here.

            return;
        }

        // Player has just landed.
        if (!wasGrounded && isGrounded)
        {
            animator.SetBool(isRunningHash, false);

            wasGrounded = true;

            // Later:
            // Landing animation goes here.

            return;
        }

        // Player is on the ground and moving left or right
        if (moveInput != 0)
        {
            animator.SetBool(isRunningHash, true);
        }
        else
        {
            animator.SetBool(isRunningHash, false);
        }

        wasGrounded = isGrounded;

    }
}
