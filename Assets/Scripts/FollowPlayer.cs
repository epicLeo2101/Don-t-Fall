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

        // Smoothly follow the player while maintaining the offset
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * smoothSpeed);

        // Smoothly rotate only on the Z-axis to match the player’s rotation
        Quaternion targetRotation = Quaternion.Euler(0, 0, player.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}
