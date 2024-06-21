using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
    
    [NonSerialized] public Rigidbody rb;
    [NonSerialized] public RagdollController rc;
    [NonSerialized] public RigBuilder rigBuilder;
    [NonSerialized] public Animator anim;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rc = gameObject.GetComponent<RagdollController>();
        rigBuilder = this.gameObject.GetComponent<RigBuilder>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        rc.DeactivateRagdoll();
    }
}
