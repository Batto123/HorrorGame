using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPart : MonoBehaviour, IShootable
{
    [NonSerialized] public GameObject mainParent;

    public void GetShot(Vector3 force)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(force * 2, ForceMode.VelocityChange);
    }
}
