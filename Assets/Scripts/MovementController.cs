using System.Collections;
using UnityEngine;
using TMPro;

public class MovementController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float rotationSpeed = 200f;
    public float jumpForce = 8f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);

        // Handle player input for movement and jumping
        HandleMovementInput();
        HandleJumpInput();
    }

    void HandleMovementInput()
    {
        // Get input values
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Check if the player wants to sprint
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Rotate the player based on input
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Determine the current speed based on whether the player is sprinting
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        // Move the player
        Vector3 movement = moveDirection * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void HandleJumpInput()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Apply vertical force for jumping
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
