using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody rb;
    private bool canMove = true;
    private bool facingRight = true;
    private bool isGrounded = true;
    private bool canJump = true;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Transform cameraTransform; // Assign the main camera in the inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Auto-assign the Main Camera (In other words you don't have to drag and drop the camera in the slot.)
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
       PlayerMovement();
    }

    public void PlayerMovement()
    {
         // Check if player is grounded
        isGrounded = Physics.OverlapSphere(groundCheck.position, checkRadius, whatIsGround).Length > 0;

        // Left and Right movement based on Camera Direction
        Vector3 camRight = cameraTransform.right;
        camRight.y = 0; // Ignore vertical tilt
        camRight.Normalize();

        moveInput = Input.GetAxis("Horizontal");
        Debug.Log(moveInput);
        Vector3 moveDirection = camRight * moveInput * moveSpeed;

        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        //Jumping check
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && canJump == true)
        {
            rb.velocity = transform.up * jumpForce;
            Debug.Log(KeyCode.Space);
            /*canJump = false;*/ // Disable jumping
        }

        // Increase Fall Velocity
        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (2f - 1f) * Time.deltaTime;
        }

        //if (isGrounded == true)
        //{
        //    StartCoroutine(JumpCooldown()); <<<<<<<------- The point here was so they can't jump immedietly the moment they land. Maybe there is a code so the charaacter 
        //}                                                      can finish its animation before enacting the jump.
    }

    //IEnumerator JumpCooldown()
    //{
    //    yield return new WaitForSeconds(1f); // Wait for 1 second
    //    canJump = true; // Re-enable jumping
    //}
}
