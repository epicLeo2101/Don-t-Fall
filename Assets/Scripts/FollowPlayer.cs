using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public float smoothSpeed = 5f; // Adjust for smoother movement
    private Vector3 offset; // Stores the initial offset

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: Player is not assigned!");
            return;
        }

        // Save the initial camera offset based on the player's starting position
        offset = transform.position - player.position;
    }

    void FixedUpdate() // Changed from LateUpdate to FixedUpdate to reduce jitter
    {
        if (player == null) return;

        // Rotate the offset only around the Z axis
        Vector3 rotatedOffset = Quaternion.Euler(0f, 0f, player.eulerAngles.z) * offset;

        // Calculate the desired camera position
        Vector3 targetPosition = player.position + rotatedOffset;

        // NEVER change the camera's Z position
        targetPosition.z = transform.position.z;

        // Smoothly follow the player
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // Only rotate around Z
        Quaternion targetRotation = Quaternion.Euler(0, 0, player.eulerAngles.z);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

    }
}