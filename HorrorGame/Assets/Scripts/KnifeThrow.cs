using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KnifeThrow : MonoBehaviour
{
    [SerializeField] private float throwPower = 10;
    [SerializeField] private float delayAfterThrow = 1f;

    [SerializeField] private Knife knife;
    private Rigidbody knifeRB;
    [SerializeField] private Transform defaultParent;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] AudioClip throwSound;

    private Ray bulletRay;
    private GameObject target;

    void Awake()
    {
        knifeRB = knife.GetComponent<Rigidbody>();
        ResetKnife();
    }

    void Update()
    {
        bool input = Input.GetKeyDown(KeyCode.Mouse0);
        
        if(input && knife.currentlyHolding)
        {
            knifeRB.isKinematic = false;
            knife.transform.parent = null;
            knife.GetComponent<BoxCollider>().enabled = true;
            knifeRB.AddForce(knife.transform.forward * throwPower, ForceMode.Impulse);
            AudioSource.PlayClipAtPoint(throwSound, transform.position);
            knife.currentlyHolding = false;
        }
        else if(input && !knife.currentlyHolding)
        {
            ResetKnife();
        }
    }

    //Das Messer quasi wieder in die "Hand" zurückbringen
    private void ResetKnife()
    {
        knifeRB.isKinematic = true;
        knife.GetComponent<BoxCollider>().enabled = false;
        knife.transform.rotation = defaultParent.transform.rotation;
        knife.transform.parent = defaultParent;
        knife.transform.localPosition = Vector3.zero;
        knife.currentlyHolding = true;
    }
}
