using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Basic3DMovment : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]float speed = 10;
    [SerializeField]GameObject cam;

    void Awake()
    {
        //Rigidbody zuweisen(Muss bei unity im gleichen Gameobject, wie der script sein)
        rb = gameObject.GetComponent<Rigidbody>();

        //Maus auf den screen locken
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Roatationinputs speichern(Die x und y werte sind absichtlich verkehrtherum)
        float y = Input.GetAxis("Mouse X") * 2;
        float x = Input.GetAxis("Mouse Y") * 2;
        //Rotation berechnen
        Vector3 rotateValue1 = new Vector3(0, y * -1, 0);
        Vector3 rotateValue2 = new Vector3(x, 0, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue1;

        //Rotation der Cam zuweisen
        cam.transform.eulerAngles = cam.transform.eulerAngles - rotateValue2;


        //Bewegungsinputs speichern
        Vector3 relativeInputs = transform.TransformDirection(new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));

        //Laufbewegung zuweisen
        rb.velocity = new(speed * relativeInputs.x, rb.velocity.y, speed * relativeInputs.z);

        
    }
}
