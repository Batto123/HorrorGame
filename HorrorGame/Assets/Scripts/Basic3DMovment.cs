using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Basic3DMovment : MonoBehaviour
{
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

    void Awake()
    {
        // Rigidbody zuweisen (Muss bei Unity im gleichen GameObject wie das Script sein)
        rb = gameObject.GetComponent<Rigidbody>();

        // Maus auf den Screen locken
        Cursor.lockState = CursorLockMode.Locked;
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
        if (Input.GetKey(sprintKey))
        {
            currentSpeed = sprintSpeed;
        }
        else
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
