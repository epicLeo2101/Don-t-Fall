using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody rb;
    //private Game_Manager manager; <<<<<<<<<<--------- This is the thing to witch to a new level

    private bool canMove = true;

    private bool facingRight = true;

    private bool isGrounded = true;
    private bool canJump = true;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is grounded
        isGrounded = Physics.OverlapSphere(groundCheck.position, checkRadius, whatIsGround).Length > 0;

        //Left and Right movement
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(moveInput * moveSpeed, rb.velocity.y);

        //Jumping check
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && canJump == true)
        {
            rb.velocity = Vector3.up * jumpForce;
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
