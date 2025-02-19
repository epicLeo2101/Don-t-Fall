using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public enum GravityDirection
    {
        PositiveX,
        NegativeX,
        PositiveY,
        NegativeY
    }

    public GravityDirection gravityDirection = GravityDirection.NegativeY; // Default gravity
    public float gravityValue = 9.81f; // Standard gravity force
    public float rotationSpeed = 5f; // Rotation smoothing speed

    private Rigidbody rb;
    private Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = transform;

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from this GameObject.");
            return;
        }

        // Lock rotation in X, Y, and Z at the start
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        UpdateGravity();
    }

    void Update()
    {
        HandleGravityInput();
    }

    void HandleGravityInput()
    {
       if (Input.GetKeyDown(KeyCode.UpArrow))
    {
        ToggleVerticalGravity();
    }
    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
        ToggleHorizontalGravity(false); // Rotates left.
    }
    else if (Input.GetKeyDown(KeyCode.RightArrow))
    {
        ToggleHorizontalGravity(true); // Rotates right.
    }
    }

    void ToggleVerticalGravity()
    {
        if (gravityDirection == GravityDirection.NegativeY)
        {
            gravityDirection = GravityDirection.PositiveY;
        }
        else if (gravityDirection == GravityDirection.PositiveY)
        {
            gravityDirection = GravityDirection.NegativeY;
        }
        else if (gravityDirection == GravityDirection.PositiveX)
        {
            gravityDirection = GravityDirection.NegativeX;
        }
        else if (gravityDirection == GravityDirection.NegativeX)
        {
            gravityDirection = GravityDirection.PositiveX;
        }

        ApplyRotation();
        UpdateGravity();
    }

    void ToggleHorizontalGravity(bool isLeft)
    {
        if (gravityDirection == GravityDirection.NegativeY)
        {
            gravityDirection = isLeft ? GravityDirection.PositiveX : GravityDirection.NegativeX;
        }
        else if (gravityDirection == GravityDirection.PositiveY)
        {
            gravityDirection = isLeft ? GravityDirection.NegativeX : GravityDirection.PositiveX;
        }
        else if (gravityDirection == GravityDirection.PositiveX)
        {
            gravityDirection = isLeft ? GravityDirection.PositiveY : GravityDirection.NegativeY;
        }
        else if (gravityDirection == GravityDirection.NegativeX)
        {
            gravityDirection = isLeft ? GravityDirection.NegativeY : GravityDirection.PositiveY;
        }

        ApplyRotation();
        UpdateGravity();
    }

    void ApplyRotation()
    {
        Quaternion targetRotation = Quaternion.identity;

        switch (gravityDirection)
        {
            case GravityDirection.PositiveX:
                targetRotation = Quaternion.Euler(0, 0, 90); // Corrected for feet-down orientation
                break;
            case GravityDirection.NegativeX:
                targetRotation = Quaternion.Euler(0, 0, -90); // Corrected for feet-down orientation
                break;
            case GravityDirection.PositiveY:
                targetRotation = Quaternion.Euler(180, 0, 0);
                break;
            case GravityDirection.NegativeY:
                targetRotation = Quaternion.Euler(0, 0, 0);
                break;
        }

        StartCoroutine(SmoothRotation(targetRotation));
    }

    IEnumerator SmoothRotation(Quaternion targetRotation)
    {
        rb.constraints = RigidbodyConstraints.None; // Unlock rotation during transition

        float elapsedTime = 0;
        Quaternion startRotation = playerTransform.rotation;

        while (elapsedTime < 1f)
        {
            playerTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        playerTransform.rotation = targetRotation; // Ensure final rotation alignment

        // Reapply constraints after rotation
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void UpdateGravity()
    {
        Vector3 gravity = Vector3.zero;

        switch (gravityDirection)
        {
            case GravityDirection.PositiveX:
                gravity = new Vector3(gravityValue, 0, 0);
                break;
            case GravityDirection.NegativeX:
                gravity = new Vector3(-gravityValue, 0, 0);
                break;
            case GravityDirection.PositiveY:
                gravity = new Vector3(0, gravityValue, 0);
                break;
            case GravityDirection.NegativeY:
                gravity = new Vector3(0, -gravityValue, 0);
                break;
        }

        Physics.gravity = gravity;

        // Reapply constraints after gravity update
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}
