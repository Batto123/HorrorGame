using System.Collections;
using UnityEngine;

public class Basic3DMovement : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody rb;
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] public GameObject cam;  // Referenz auf die Kamera
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] float sprintSpeed = 18f;
    float currentSpeed;
    [SerializeField] public KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float jumpForce = 5f;
    public bool isGrounded;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] float crouchSpeed = 5f;
    [SerializeField] float crouchYScale = 0.5f;
    [SerializeField] bool canStandUp = true;

    public float maxSlopeAngle = 45f;
    private RaycastHit slopeHit;
    private float playerHeight = 2f;
    private Vector3 moveDirection;

    bool crouching;
    float startYScale;

    // Drehgeschwindigkeit für die Maus
    [SerializeField] float mouseSensitivity = 2f;  
    private float rotationY = 0f;  // Speicherung der vertikalen Rotation

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true; // Ensure gravity is enabled initially
        rb.drag = 0; // Ensure drag is zero for normal falling behavior

        Cursor.lockState = CursorLockMode.Locked;
        startYScale = transform.localScale.y;
    }

    void Update()
    {
        // Mausrotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontale Rotation
        transform.Rotate(Vector3.up * mouseX);

        // Vertikale Rotation
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -45f, 45f); // Begrenzung der vertikalen Rotation
        cam.transform.localEulerAngles = new Vector3(rotationY, 0, 0);

        // Bewegung
        moveDirection = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));

        if (Input.GetKey(sprintKey) && !crouching)
        {
            currentSpeed = sprintSpeed;
        }
        else if (!crouching)
        {
            currentSpeed = walkSpeed;
        }

        // Block movement in the direction of walls
        if (CheckWallCollisions())
        {
            moveDirection = Vector3.zero; // Stop movement if a wall is detected in the move direction
        }

        if (OnSlope() && !Input.GetKey(jumpKey))
        {
            rb.velocity = GetSlopeMoveDirection() * currentSpeed;
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
            rb.velocity = new Vector3(currentSpeed * moveDirection.x, rb.velocity.y, currentSpeed * moveDirection.z);
        }

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.useGravity = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            crouching = !crouching;

            if (crouching)
            {
                Crouch();
            }
            else if (canStandUp)
            {
                StandUp();
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.useGravity = !OnSlope();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Crouch()
    {
        crouching = true;
        currentSpeed = crouchSpeed;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    void StandUp()
    {
        crouching = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        currentSpeed = walkSpeed;
    }

    public bool IsSprinting()
    {
        return Input.GetKey(sprintKey) && !crouching && IsMoving();
    }

    public bool IsCrouching()
    {
        return crouching;
    }

    public bool IsWalking()
    {
        return rb.velocity.magnitude > 0.1f && !IsSprinting() && !IsCrouching();
    }

    public bool IsMoving()
    {
        return moveDirection.magnitude > 0.1f;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private bool CheckWallCollisions()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, 0.6f))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return true;
            }
        }
        return false;
    }
}
