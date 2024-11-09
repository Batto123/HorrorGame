using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [NonSerialized] public bool currentlyHolding;

    private Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision coll)
    {
        float speed = (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y) + Mathf.Abs(rb.velocity.z)) / 3;
        Debug.Log(speed);

        if(speed > 3 && coll.gameObject.TryGetComponent<IShootable>(out IShootable shootable))
        {
            shootable.GetShot();
        }
    }
}
