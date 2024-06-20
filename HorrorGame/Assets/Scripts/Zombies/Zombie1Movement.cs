using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zombie1Movement : MonoBehaviour
{
    Rigidbody rb;
    ZombieMain mainScript;

    [SerializeField] float walkSpeed = 2;
    [SerializeField] float playerCheckRadius = 5;
    [SerializeField] LayerMask playerLayer;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        mainScript = gameObject.GetComponent<ZombieMain>();
    }

    void Update()
    {
        if(mainScript.moveState == ZombieMain.ZombieMoveStates.IDLE)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerCheckRadius, playerLayer);
            if(colliders.Length > 0)
            {
                mainScript.moveState = ZombieMain.ZombieMoveStates.WALK;
            }
        }
        else if(mainScript.moveState == ZombieMain.ZombieMoveStates.WALK)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerCheckRadius, playerLayer);
            if(colliders.Length == 0)
            {
                rb.velocity = Vector3.zero;

                mainScript.moveState = ZombieMain.ZombieMoveStates.IDLE;
            }else
            {
                transform.LookAt(colliders[0].gameObject.transform);
                transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0));

                rb.velocity = transform.forward * walkSpeed;
            }

            
        }
    }

}
