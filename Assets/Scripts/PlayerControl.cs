using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 25f;
    public float deceleration = 20f;

    [Header("Jump")]
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Gravity")]
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
            Debug.LogError(
                "PlayerControl could not find a GravityController."
            );
        }
    }

    void Update()
    {
        GetInput();
        CheckGrounded();
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void GetInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) &&
            isGrounded &&
            canJump)
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

        Vector3 upDirection =
            gravityController.GetUpDirection();

        // -------------------------------------------------
        // CAMERA MOVEMENT
        // -------------------------------------------------

        Vector3 camRight = cameraTransform.right;

        // Project camera-right onto the player's current
        // walking surface.
        camRight = Vector3.ProjectOnPlane(
            camRight,
            upDirection
        ).normalized;

        Vector3 targetMovement =
            camRight * moveInput * moveSpeed;

        // -------------------------------------------------
        // CURRENT VELOCITY
        // -------------------------------------------------

        Vector3 currentVelocity = rb.velocity;

        // Separate velocity into:
        //
        // 1. Gravity velocity
        // 2. Surface/momentum velocity
        //
        // Anything perpendicular to gravity is preserved.
        Vector3 gravityVelocity =
            Vector3.Project(
                currentVelocity,
                -upDirection
            );

        Vector3 surfaceVelocity =
            Vector3.ProjectOnPlane(
                currentVelocity,
                upDirection
            );

        // -------------------------------------------------
        // MOVEMENT + MOMENTUM
        // -------------------------------------------------

        float accelerationRate;

        if (moveInput != 0)
        {
            accelerationRate = acceleration;
        }
        else
        {
            accelerationRate = deceleration;
        }

        surfaceVelocity = Vector3.MoveTowards(
            surfaceVelocity,
            targetMovement,
            accelerationRate * Time.fixedDeltaTime
        );

        // -------------------------------------------------
        // COMBINE VELOCITIES
        // -------------------------------------------------

        rb.velocity =
            gravityVelocity +
            surfaceVelocity;
    }

    void Jump()
    {
        if (gravityController == null)
            return;

        Vector3 upDirection =
            gravityController.GetUpDirection();

        // Preserve momentum that is parallel to the ground.
        Vector3 surfaceVelocity =
            Vector3.ProjectOnPlane(
                rb.velocity,
                upDirection
            );

        // Jump opposite the direction of gravity.
        rb.velocity =
            surfaceVelocity +
            upDirection * jumpForce;
    }
}

