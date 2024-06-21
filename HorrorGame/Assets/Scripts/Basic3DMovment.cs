using System.Collections;
using UnityEngine;

public class Basic3DMovment : MonoBehaviour
{
    [Header("Movement")]
    // Spieler Rigidbody
    public Rigidbody rb;
    // Gehen
    [SerializeField] float walkSpeed = 10f;
    // Kamera Bewegung
    [SerializeField] public GameObject cam; // Zugriffsschutz geändert zu public
    // Sprinten
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] float sprintSpeed = 18f;
    float currentSpeed;
    // Springen
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float jumpForce = 5f;
    // Überprüfen, ob der Spieler am Boden ist
    bool isGrounded;
    // Ducken
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] float crouchSpeed = 5f;
    [SerializeField] float crouchYScale = 0.5f;
    [SerializeField] bool canStandUp = true; // Serialized Bool für Aufstehen

    // Slope Handling
    public float maxSlopeAngle = 45f;
    private RaycastHit slopeHit;
    private float playerHeight = 2f; // Höhe des Spielers
    private Vector3 moveDirection;

    bool crouching;
    float startYScale;

    void Awake()
    {
        // Rigidbody zuweisen (Muss bei Unity im gleichen GameObject wie das Script sein)
        rb = gameObject.GetComponent<Rigidbody>();

        // Maus auf den Screen locken
        Cursor.lockState = CursorLockMode.Locked;

        // Startskala speichern
        startYScale = transform.localScale.y;
    }

    void Update()
    {
        // Rotationinputs speichern
        float y = Input.GetAxis("Mouse X") * 2;
        float x = Input.GetAxis("Mouse Y") * 2;

        // Rotation berechnen
        Vector3 rotateValue1 = new Vector3(0, y, 0);
        Vector3 rotateValue2 = new Vector3(-x, 0, 0);
        transform.eulerAngles += rotateValue1;

        // Rotation der Kamera zuweisen
        cam.transform.localEulerAngles += rotateValue2;

        // Bewegungsinputs speichern
        moveDirection = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));

        // Prüfen, ob die Sprinttaste gedrückt ist
        if (Input.GetKey(sprintKey) && !crouching)
        {
            currentSpeed = sprintSpeed;
        }
        else if (!crouching)
        {
            currentSpeed = walkSpeed;
        }

        // Laufbewegung zuweisen
        if (OnSlope() && !Input.GetKey(jumpKey))
        {
            rb.velocity = GetSlopeMoveDirection() * currentSpeed;
            rb.useGravity = false; // Schwerkraft deaktivieren, wenn auf dem Hang
        }
        else
        {
            rb.velocity = new Vector3(currentSpeed * moveDirection.x, rb.velocity.y, currentSpeed * moveDirection.z);
            rb.useGravity = true; // Schwerkraft aktivieren
        }

        // Prüfen, ob die Sprungtaste gedrückt wurde und der Spieler auf dem Boden ist
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.useGravity = true; // Schwerkraft aktivieren, um zu springen
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Toggle für Ducken/Aufstehen
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

    // Überprüfen, ob der Spieler den Boden berührt
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.useGravity = !OnSlope(); // Schwerkraft basierend auf dem Hangstatus setzen
        }
    }

    // Kombinierte OnCollisionExit Methode
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Ducken
    void Crouch()
    {
        crouching = true;
        currentSpeed = crouchSpeed;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    // Aufstehen
    void StandUp()
    {
        crouching = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        currentSpeed = walkSpeed;
    }

    // Methode zur Überprüfung, ob der Spieler sprintet
    public bool IsSprinting()
    {
        return Input.GetKey(sprintKey) && !crouching && IsMoving();
    }

    // Methode zur Überprüfung, ob der Spieler geduckt ist
    public bool IsCrouching()
    {
        return crouching;
    }

    // Methode zur Überprüfung, ob der Spieler geht
    public bool IsWalking()
    {
        return rb.velocity.magnitude > 0.1f && !IsSprinting() && !IsCrouching();
    }

    // Methode zur Überprüfung, ob der Spieler sich bewegt
    public bool IsMoving()
    {
        return moveDirection.magnitude > 0.1f;
    }

    // Überprüfen, ob der Spieler sich auf einem Hang befindet
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    // Richtung in der sich der Spieler auf einem Hang bewegt
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
