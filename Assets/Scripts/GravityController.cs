using System.Collections;
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

    [Header("Gravity")]
    public GravityDirection gravityDirection = GravityDirection.NegativeY;
    public float gravityValue = 9.81f;

    [Header("Rotation")]
    public float rotationSpeed = 5f;

    private Rigidbody rb;
    private Coroutine rotationCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("GravityController requires a Rigidbody.");
            enabled = false;
            return;
        }

        // We use Unity's Physics.gravity.
        rb.useGravity = true;

        // Prevent physics from spinning the player.
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        UpdateGravity();
        ApplyRotation();
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
            ToggleHorizontalGravity(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ToggleHorizontalGravity(true);
        }
    }

    void ToggleVerticalGravity()
    {
        switch (gravityDirection)
        {
            case GravityDirection.NegativeY:
                gravityDirection = GravityDirection.PositiveY;
                break;

            case GravityDirection.PositiveY:
                gravityDirection = GravityDirection.NegativeY;
                break;

            case GravityDirection.PositiveX:
                gravityDirection = GravityDirection.NegativeX;
                break;

            case GravityDirection.NegativeX:
                gravityDirection = GravityDirection.PositiveX;
                break;
        }

        UpdateGravity();
        ApplyRotation();
    }

    void ToggleHorizontalGravity(bool isLeft)
    {
        switch (gravityDirection)
        {
            case GravityDirection.NegativeY:
                gravityDirection = isLeft
                    ? GravityDirection.PositiveX
                    : GravityDirection.NegativeX;
                break;

            case GravityDirection.PositiveY:
                gravityDirection = isLeft
                    ? GravityDirection.NegativeX
                    : GravityDirection.PositiveX;
                break;

            case GravityDirection.PositiveX:
                gravityDirection = isLeft
                    ? GravityDirection.PositiveY
                    : GravityDirection.NegativeY;
                break;

            case GravityDirection.NegativeX:
                gravityDirection = isLeft
                    ? GravityDirection.NegativeY
                    : GravityDirection.PositiveY;
                break;
        }

        UpdateGravity();
        ApplyRotation();
    }

    void UpdateGravity()
    {
        Physics.gravity = GetGravityDirection() * gravityValue;
    }

    public Vector3 GetGravityDirection()
    {
        switch (gravityDirection)
        {
            case GravityDirection.PositiveX:
                return Vector3.right;

            case GravityDirection.NegativeX:
                return Vector3.left;

            case GravityDirection.PositiveY:
                return Vector3.up;

            case GravityDirection.NegativeY:
                return Vector3.down;
        }

        return Vector3.down;
    }

    public Vector3 GetUpDirection()
    {
        return -GetGravityDirection();
    }

    void ApplyRotation()
    {
        Quaternion targetRotation;

        switch (gravityDirection)
        {
            case GravityDirection.PositiveX:
                targetRotation = Quaternion.Euler(0f, 0f, 90f);
                break;

            case GravityDirection.NegativeX:
                targetRotation = Quaternion.Euler(0f, 0f, -90f);
                break;

            case GravityDirection.PositiveY:
                targetRotation = Quaternion.Euler(180f, 0f, 0f);
                break;

            case GravityDirection.NegativeY:
            default:
                targetRotation = Quaternion.identity;
                break;
        }

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        rotationCoroutine = StartCoroutine(
            SmoothRotation(targetRotation)
        );
    }

    IEnumerator SmoothRotation(Quaternion targetRotation)
    {
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;

            transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                elapsedTime
            );

            yield return null;
        }

        transform.rotation = targetRotation;

        rotationCoroutine = null;
    }
}
