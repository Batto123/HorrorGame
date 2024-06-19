using System.Collections;
using System.Collections.Generic;
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
        // Rotationinputs speichern (Die x- und y-Werte sind absichtlich verkehrtherum)
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
        else if(!crouching)
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

        // Wenn die Ducken-Taste gedrückt wird, ducken
        if (Input.GetKeyDown(crouchKey))
        {
            crouching = true;
            currentSpeed = crouchSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        // Aufstehen, wenn die Ducken-Taste losgelassen wird
        if (Input.GetKeyUp(crouchKey))
        {
            crouching = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            currentSpeed = walkSpeed;
        }
    }

    // Überprüfen, ob der Spieler den Boden berührt
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
