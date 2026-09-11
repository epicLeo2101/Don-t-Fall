using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Model")]
    public Transform modelPivot;

    [Header("Facing")]
    public float rotationSpeed = 10f;

    [Header("References")]
    public GravityController gravityController;

    private PlayerControl playerControl;

    private float previousInput;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();

        if (gravityController == null)
        {
            gravityController = GetComponent<GravityController>();
        }

        if (modelPivot == null)
        {
            Debug.LogError("PlayerAnimation: Model Pivot is not assigned!");
        }

        if (playerControl == null)
        {
            Debug.LogError("PlayerAnimation: PlayerControl not found!");
        }
    }

    void Update()
    {
        FaceInputDirection();
    }

    void FaceInputDirection()
    {
        if (modelPivot == null || gravityController == null || playerControl == null)
            return;

        // Do not change facing while airborne.
        if (!playerControl.IsGrounded())
            return;

        float moveInput = Input.GetAxisRaw("Horizontal");

        // No input.
        if (moveInput == 0)
        {
            previousInput = 0;
            return;
        }

        // Turn as soon as input changes direction.
        if (moveInput != previousInput)
        {
            TurnToInputDirection(moveInput);
        }

        previousInput = moveInput;
    }

    void TurnToInputDirection(float moveInput)
    {
        Vector3 upDirection =
            gravityController.GetUpDirection();

        Vector3 direction =
            Camera.main.transform.right * moveInput;

        direction = Vector3.ProjectOnPlane(
            direction,
            upDirection
        ).normalized;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Vector3 localDirection =
            transform.InverseTransformDirection(direction);

        float angle =
            Mathf.Atan2(
                localDirection.x,
                localDirection.z
            ) * Mathf.Rad2Deg;

        Quaternion targetRotation =
            Quaternion.Euler(0f, angle, 0f);

        modelPivot.localRotation = targetRotation;
    }


}
