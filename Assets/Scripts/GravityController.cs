using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{

    // Enum to define the possible gravity directions

    public enum GravityDirection

    {

        PositiveX,

        NegativeX,

        PositiveY,

        NegativeY

    }

    // Public variables to set the gravity direction and value in the Inspector

    public GravityDirection gravityDirection = GravityDirection.NegativeY; // Default direction

    public float gravityValue = 2f; // Magnitude of gravity



    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component

        rb = GetComponent<Rigidbody>();

        if (rb == null)

        {

            Debug.LogError("Rigidbody component is missing from this GameObject.");

            return;

        }



        // Initialize gravity and constraints

        UpdateGravity();
    }

    // Update is called once per frame
    void Update()
    {
        // Update gravity and constraints if the direction or value changes

        UpdateGravity();
    }

    void UpdateGravity()

    {

        // Determine the direction of gravity based on gravityDirection

        Vector3 gravity = Vector3.zero;


        switch (gravityDirection)

        {

            case GravityDirection.PositiveX:

                gravity = new Vector3(gravityValue, 0);

                break;

            case GravityDirection.NegativeX:

                gravity = new Vector3(-gravityValue, 0);

                break;

            case GravityDirection.PositiveY:

                gravity = new Vector3(0, gravityValue);

                break;

            case GravityDirection.NegativeY:

                gravity = new Vector3(0, -gravityValue);

                break;

        }



        // Apply the new gravity direction and to the Physics3D settings and Rigidbody2D

        Physics.gravity = gravity;

    }
}
