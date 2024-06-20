using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMain : MonoBehaviour
{
    public enum ZombieMoveStates
    {
        IDLE,
        WALK,
        ATTACK,
        HURT,
        DEAD
    }

    public ZombieMoveStates moveState = ZombieMoveStates.IDLE;
    
    [NonSerialized] public Animator anim;
    [NonSerialized] public Rigidbody rb;
    [NonSerialized] public RagdollController rc;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        rc = gameObject.GetComponent<RagdollController>();   
    }

    void Start()
    {
        rc.DeactivateRagdoll();
    }
}
