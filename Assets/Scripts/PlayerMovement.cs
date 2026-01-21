using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Animator animator;

    [Header("Movement Settings")]
    public float speed = 8f;
    public float rotationSpeed = 10f; // Higher = Snappier, Lower = Smoother
    public float gravity = -25f;
    public float jumpHeight = 2.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        // 1. Grounding Logic
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Input Handling (New Input System)
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y = -1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x = 1;
        }

        // Normalize prevents fast diagonal movement
        Vector3 move = new Vector3(input.x, 0, input.y).normalized;

        // 3. Movement & Smooth Rotation
        if (move.magnitude >= 0.1f)
        {
            // Calculate the target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            
            // Smoothly rotate towards that target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            controller.Move(move * speed * Time.deltaTime);
        }

        // 4. Jumping
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        // 5. Physics & Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 6. Animation (with Damping for smoothness)
        float horizontalSpeed = move.magnitude; 
        animator.SetFloat("Speed", horizontalSpeed, 0.05f, Time.deltaTime);
        animator.SetBool("isGrounded", isGrounded);
    }
}