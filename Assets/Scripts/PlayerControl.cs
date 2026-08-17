using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("References")]
    public GravityController gravityController;

    private Rigidbody rb;

    private float moveInput;
    private bool isGrounded;
    private bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (gravityController == null)
        {
            gravityController = GetComponent<GravityController>();
        }

        if (gravityController == null)
        {
            Debug.LogError("PlayerControl could not find a GravityController.");
        }
    }

    void Update()
    {
        HandleInput();
        CheckGrounded();
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            Jump();
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics.OverlapSphere(
            groundCheck.position,
            checkRadius,
            whatIsGround
        ).Length > 0;
    }

    void PlayerMovement()
    {
        if (gravityController == null)
            return;

        // Current direction of gravity.
        Vector3 gravityDirection =
            gravityController.GetGravityDirection();

        // Direction opposite gravity.
        Vector3 upDirection =
            gravityController.GetUpDirection();

        // Get camera's right direction.
        Vector3 camRight = cameraTransform.right;

        // Project camera right onto the player's current ground plane.
        camRight = Vector3.ProjectOnPlane(
            camRight,
            upDirection
        ).normalized;

        // Horizontal movement along the current surface.
        Vector3 movementVelocity =
            camRight * moveInput * moveSpeed;

        // Preserve the velocity caused by gravity.
        Vector3 gravityVelocity =
            Vector3.Project(rb.velocity, gravityDirection);

        // Combine movement + gravity.
        rb.velocity = movementVelocity + gravityVelocity;

        // Extra falling acceleration.
        if (!isGrounded)
        {
            float gravitySpeed =
                Vector3.Dot(rb.velocity, gravityDirection);

            if (gravitySpeed > 0)
            {
                rb.AddForce(
                    gravityDirection * Physics.gravity.magnitude,
                    ForceMode.Acceleration
                );
            }
        }
    }

    void Jump()
    {
        if (gravityController == null)
            return;

        Vector3 upDirection =
            gravityController.GetUpDirection();

        // Remove current velocity along the gravity axis.
        Vector3 velocityWithoutGravity =
            Vector3.ProjectOnPlane(
                rb.velocity,
                upDirection
            );

        // Jump opposite the direction of gravity.
        rb.velocity =
            velocityWithoutGravity +
            upDirection * jumpForce;
    }
}

