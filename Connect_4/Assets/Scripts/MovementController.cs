using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 2f;
    private Vector3 velocityVector = Vector3.zero;
    private Rigidbody rb;
    public float maxVelocityChange = 4f;
    public float tiltAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get joystck input
        float _xMovement = joystick.Horizontal;
        float _zMovement = joystick.Vertical;

        // Calculating velocity vector
        Vector3 _movementHorizontal = transform.right * _xMovement;
        Vector3 _movementVertical = transform.forward * _zMovement;

        // Final movement velocity vector
        Vector3 _movementVelocityVector = (_movementHorizontal + _movementVertical).normalized * speed;

        Move(_movementVelocityVector);

        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -1 * joystick.Horizontal * speed * tiltAmount);

    }
    
    // Sets velocity for object
    void Move(Vector3 movement)
    {
        velocityVector = movement;
    }

    // Applies velocity to the rigidbody
    private void FixedUpdate()
    {
        if(velocityVector != Vector3.zero)
        {
            // Get rigidbodys current velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);

            // Apply a force by the amount of velocity change to reach the target velocity
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0f;

            // Adds force to rigidbody
            rb.AddForce(velocityChange, ForceMode.Acceleration);

        }

        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -1 * joystick.Horizontal * speed * tiltAmount);
    }
}
