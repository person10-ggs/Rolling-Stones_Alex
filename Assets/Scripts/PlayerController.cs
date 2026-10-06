using UnityEngine;

/*
 * GameObject: Player (Ball)
 * Required Dependencies: Rigidbody, SphereCollider
 * Description: Manages rolling physics movement relative to camera orientation, Q/E camera pivot rotation, and a spacebar dash ability governed by a cooldown timer.
 * VERSION 1.0: Implemented WASD ball movement, Q/E camera rotation controls, and directional dash system with cooldown mechanics.
 */
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class PlayerController : MonoBehaviour
{
    // Serialized Fields visible in the Unity Inspector
    [Header("Movement Settings")]
    [Tooltip("The amount of force applied to roll the ball.")]
    [SerializeField] private float moveForce = 15f;

    [Tooltip("The rotation speed of the camera pivot in degrees per second.")]
    [SerializeField] private float cameraRotationSpeed = 100f;

    [Header("Dash Settings")]
    [Tooltip("The impulse force applied when performing a dash.")]
    [SerializeField] private float dashForce = 25f;

    [Tooltip("The duration in seconds required between consecutive dashes.")]
    [SerializeField] private float dashCooldown = 2f;

    [Header("Camera References")]
    [Tooltip("Reference to the Camera transform or Camera Pivot transform used to determine the forward direction.")]
    [SerializeField] private Transform cameraTransform;

    // Private Component and State Variables
    private Rigidbody rb;
    private float moveHorizontal;
    private float moveVertical;
    private float rotationInput;
    private bool isDashRequested;
    private float currentDashCooldown;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Cache the attached Rigidbody component for efficient physics operations
        rb = GetComponent<Rigidbody>();
    }

    // Called once per frame to process user inputs and timers
    private void Update()
    {
        ReadInput();
        UpdateCooldowns();
    }

    // Called at fixed physics intervals to apply movement forces
    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyCameraRotation();
        ApplyDash();
    }

    // Captures keyboard input for movement, camera rotation, and dash
    private void ReadInput()
    {
        // Store movement axis inputs (-1.0 to 1.0)
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        // Process camera rotation input using Q and E keys
        rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationInput -= 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationInput += 1f;
        }

        // Trigger dash request on Space key press if dash is off cooldown
        if (Input.GetKeyDown(KeyCode.Space) && currentDashCooldown <= 0f)
        {
            isDashRequested = true;
        }
    }

    // Decrements active cooldown timers over real-time seconds
    private void UpdateCooldowns()
    {
        // Reduce cooldown timer if currently active
        if (currentDashCooldown > 0f)
        {
            currentDashCooldown -= Time.deltaTime;
        }
    }

    // Applies rolling force based on the current orientation of the camera
    private void ApplyMovement()
    {
        // Determine reference direction based on camera or default forward
        Vector3 forward = cameraTransform != null ? cameraTransform.forward : transform.forward;
        Vector3 right = cameraTransform != null ? cameraTransform.right : transform.right;

        // Flatten direction vectors onto the horizontal plane to prevent unwanted vertical forces
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate continuous movement direction vector
        Vector3 moveDirection = (forward * moveVertical + right * moveHorizontal).normalized;

        // Apply force to the Rigidbody physics object
        rb.AddForce(moveDirection * moveForce, ForceMode.Force);
    }

    // Rotates the camera transform around the Y axis when Q/E are pressed
    private void ApplyCameraRotation()
    {
        // Ensure camera transform reference exists before attempting rotation
        if (cameraTransform != null && rotationInput != 0f)
        {
            // Rotate camera pivot around world Y-axis based on rotation speed and physics delta time
            cameraTransform.Rotate(Vector3.up, rotationInput * cameraRotationSpeed * Time.fixedDeltaTime, Space.World);
        }
    }

    // Executes impulse dash force and resets cooldown
    private void ApplyDash()
    {
        // Check if dash was requested during Update
        if (isDashRequested)
        {
            // Reset request flag
            isDashRequested = false;

            // Determine reference direction based on camera or default transform
            Vector3 forward = cameraTransform != null ? cameraTransform.forward : transform.forward;
            Vector3 right = cameraTransform != null ? cameraTransform.right : transform.right;

            // Flatten reference vectors to horizontal plane
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // Determine dash direction; default to forward if no WASD input is active
            Vector3 inputDirection = (forward * moveVertical + right * moveHorizontal).normalized;
            Vector3 dashDirection = inputDirection != Vector3.zero ? inputDirection : forward;

            // Apply immediate impulse force in the calculated dash direction
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

            // Start cooldown timer
            currentDashCooldown = dashCooldown;
        }
    }
}