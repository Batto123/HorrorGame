using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePhysics : MonoBehaviour
{
    [NonSerialized] private ZombieMain main;

    public bool isGrounded {get; set;}

    void Awake()
    {
        main = gameObject.GetComponent<ZombieMain>();
        isGrounded = true;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || OnSlope())
        {
            isGrounded = true;
            main.rb.useGravity = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            main.rb.useGravity = true;
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2 * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < 45 && angle != 0;
        }
        return false;
    }
}
