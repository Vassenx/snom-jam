using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public bool canMove { get; private set; } = true;
    public bool canJump { get; private set; } = true;
    private Vector3 curInputDirection;
    
    [SerializeField] private float swimSpeed = 3.0f;
    [SerializeField] private float jumpSpeed = 3.0f;
    [SerializeField] private float rotSpeed = 2.0f;
    [SerializeField] private float gravity = 30.0f;
   
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        curInputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            HandleJump();
            HandleBasicMovement();
            HandleGravity();
            RotateToInput();
        }
    }
    
    private void HandleBasicMovement()
    {
        var curVelocity = swimSpeed * curInputDirection;
        var velocityChange = rb.velocity - curVelocity;

        rb.AddForce(curVelocity, ForceMode.VelocityChange);
        
        rb.velocity = new Vector3(
            Mathf.Clamp(rb.velocity.x, -10f, 10f), 
            rb.velocity.y,
            Mathf.Clamp(rb.velocity.y, -10f, 10f));
    }

    private bool HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(jumpSpeed * transform.up.normalized, ForceMode.Impulse);
            return true;
        }

        return false;
    }

    private void HandleGravity()
    {
        rb.AddForce(new Vector3 (0, -gravity, 0), ForceMode.Acceleration);
    }

    private void RotateToInput()
    {
        if (curInputDirection.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (curInputDirection), Time.deltaTime * rotSpeed);
        }
    }
}
