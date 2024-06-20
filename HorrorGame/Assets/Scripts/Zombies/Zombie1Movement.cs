using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zombie1Movement : MonoBehaviour
{
    ZombieMain mainScript;
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float playerCheckRadius = 5;
    [SerializeField] LayerMask playerLayer;

    void Awake()
    {
        mainScript = gameObject.GetComponent<ZombieMain>();
    }

    void Update()
    {
        if(mainScript.moveState == ZombieMain.ZombieMoveStates.IDLE)
        {
            mainScript.anim.Play("idle");

            Collider[] colliders = Physics.OverlapSphere(transform.position, playerCheckRadius, playerLayer);
            if(colliders.Length > 0)
            {
                mainScript.moveState = ZombieMain.ZombieMoveStates.WALK;
            }
        }
        else if(mainScript.moveState == ZombieMain.ZombieMoveStates.WALK)
        {
            mainScript.anim.Play("walk");

            Collider[] colliders = Physics.OverlapSphere(transform.position, playerCheckRadius, playerLayer);
            if(colliders.Length == 0)
            {
                mainScript.rb.velocity = Vector3.zero;

                mainScript.moveState = ZombieMain.ZombieMoveStates.IDLE;

            }else
            {
                transform.LookAt(colliders[0].gameObject.transform);
                transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0));

                mainScript.rb.velocity = transform.forward * walkSpeed;
            }

            
        }
    }

}
