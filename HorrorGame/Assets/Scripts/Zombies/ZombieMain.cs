using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ZombieMain : MonoBehaviour
{
    //Verweise
    [NonSerialized] public Rigidbody rb;
    [NonSerialized] public RagdollController rc;
    [NonSerialized] public RigBuilder rigBuilder;
    [NonSerialized] public Animator anim;
    [NonSerialized] public Material material;

    //State zeug
    public BaseState CurrentState {get; set;}
    public readonly ZombieIdleState IdleState = new();
    public readonly ZombieWalkState WalkState = new();
    public readonly ZombieAttackState AttackState = new();
    public readonly ZombieHurtState HurtState = new();
    public readonly ZombieDeadState DeadState = new();
    
    //Variablen / Eigenschaften
    public LayerMask playerLayer;
    public int currentLifes = 3;
    public float viewRadius = 8f;
    public float attackRange = 2f;
    public float walkSpeed = 0.9f;
    public float hitStun = 0.2f;
    public AudioSource mainSound;
    public AudioSource attackSound;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rc = gameObject.GetComponent<RagdollController>();
        rigBuilder = gameObject.GetComponent<RigBuilder>();
        anim = gameObject.GetComponent<Animator>();
        material = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;

        CurrentState = IdleState;
        CurrentState.Enter(this, IdleState);
    }
    
    void Start()
    {
        rc.DeactivateRagdoll();
    }

    void Update()
    {
        CurrentState.Update(this);
    }

    public void SwitchState(BaseState state)
    {
        CurrentState.Leave(this);
        state.Enter(this, CurrentState);
        CurrentState = state;
        
    }

    public Coroutine StartTimer(float waitingTime, Action<ZombieMain> trigger){return StartCoroutine(_StartTimer(waitingTime, trigger));}
    private IEnumerator _StartTimer(float waitingTime, Action<ZombieMain> trigger)
    {
        yield return new WaitForSeconds(waitingTime);
        trigger(this);
    }

    public void CancelTimer(Coroutine timer){StopCoroutine(timer);}

    
}
