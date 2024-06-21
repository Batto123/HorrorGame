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
            mainScript.anim.Play("idleZombie1");
            mainScript.rigBuilder.enabled = false;

            Collider[] colliders = Physics.OverlapSphere(transform.position, playerCheckRadius, playerLayer);
            if(colliders.Length > 0)
            {
                mainScript.rigBuilder.enabled = true;
                mainScript.moveState = ZombieMain.ZombieMoveStates.WALK;
            }
        }
        else if(mainScript.moveState == ZombieMain.ZombieMoveStates.WALK)
        {
            mainScript.anim.Play("walkZombie1");

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
