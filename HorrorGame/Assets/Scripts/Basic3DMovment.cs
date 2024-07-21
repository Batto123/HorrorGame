using System.Collections;
using UnityEngine;

public class Basic3DMovment : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody rb;
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] public GameObject cam;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] float sprintSpeed = 18f;
    float currentSpeed;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float jumpForce = 5f;
    bool isGrounded;
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
        float y = Input.GetAxis("Mouse X") * 2;
        float x = Input.GetAxis("Mouse Y") * 2;

        Vector3 rotateValue1 = new Vector3(0, y, 0);
        Vector3 rotateValue2 = new Vector3(-x, 0, 0);
        transform.eulerAngles += rotateValue1;
        cam.transform.localEulerAngles += rotateValue2;

        moveDirection = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));

        if (Input.GetKey(sprintKey) && !crouching)
        {
            currentSpeed = sprintSpeed;
        }
        else if (!crouching)
        {
            currentSpeed = walkSpeed;
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
}
