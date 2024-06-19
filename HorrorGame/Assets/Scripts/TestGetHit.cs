using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetHit : MonoBehaviour, IShootable
{
    Rigidbody rb;
    
    //Sucht im GameObject nach nem Rigidbody, den er zuweisen kann
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    //Schießt das GameObject etwas weg, wenn es getroffen wird
    public void GetShot(Vector3 knockback = default(Vector3))
    {
        rb.AddForce(knockback, ForceMode.VelocityChange);
    }
}
