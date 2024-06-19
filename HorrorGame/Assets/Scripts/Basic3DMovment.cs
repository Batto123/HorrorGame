using System.Collections;
using UnityEngine;

public class Basic3DMovment : MonoBehaviour
{
    [Header("Movement")]
    // Spieler Rigidbody
    Rigidbody rb;
    // Gehen
    [SerializeField] float walkSpeed = 10;
    // Kamera Bewegung
    [SerializeField] GameObject cam;
    // Sprinten
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] float sprintSpeed = 18;
    float currentSpeed;
    // Springen
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float jumpForce = 5;
    // Überprüfen, ob der Spieler am Boden ist
    bool isGrounded;
    // Ducken
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] float crouchSpeed = 5;
    [SerializeField] float crouchYScale = 0.5f;
    [SerializeField] bool canStandUp = true; // Serialized Bool für Aufstehen

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
        // Rotationinputs speichern (Die x- und y-Werte sind absichtlich verkehrt herum)
        float y = Input.GetAxis("Mouse X") * 2;
        float x = Input.GetAxis("Mouse Y") * 2;

        // Rotation berechnen
        Vector3 rotateValue1 = new Vector3(0, y * -1, 0);
        Vector3 rotateValue2 = new Vector3(x, 0, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue1;

        // Rotation der Kamera zuweisen
        cam.transform.eulerAngles = cam.transform.eulerAngles - rotateValue2;

        // Bewegungsinputs speichern
        Vector3 relativeInputs = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));

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
        rb.velocity = new Vector3(currentSpeed * relativeInputs.x, rb.velocity.y, currentSpeed * relativeInputs.z);

        // Prüfen, ob die Sprungtaste gedrückt wurde und der Spieler auf dem Boden ist
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
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

        // Aktualisierung des canStandUp-Bools basierend auf der Roof-Detection
        UpdateCanStandUp();
    }

    // Überprüfen, ob der Spieler den Boden berührt
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    // Kombinierte OnCollisionExit Methode
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }

        if (collision.gameObject.tag == "Roof" && crouching)
        {
            canStandUp = true; // Spieler kann aufstehen, wenn er nicht mehr unter dem Roof ist
        }
    }

    // Überprüfen, ob der Spieler unter einem Roof-Tag-Objekt ist
    bool IsUnderRoof()
    {
        RaycastHit hit;
        float distanceToRoof = crouchYScale + 0.5f; // 0.5f über dem geduckten Spieler

        // Raycast von der Spielerposition nach oben
        if (Physics.Raycast(transform.position, Vector3.up, out hit, distanceToRoof))
        {
            if (hit.collider.CompareTag("Roof"))
            {
                return true;
            }
        }
        return false;
    }

    // Aktualisiert den canStandUp-Bool basierend auf der aktuellen Situation
    void UpdateCanStandUp()
    {
        canStandUp = !IsUnderRoof();
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
}