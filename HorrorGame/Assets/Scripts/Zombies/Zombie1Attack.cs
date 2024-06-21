using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1Attack : MonoBehaviour
{
    ZombieMain mainScript;

    [SerializeField] float playerCheckRadius = 1;
    [SerializeField] LayerMask playerLayer;

    [SerializeField] private AudioSource attackSound;

    void Awake()
    {
        mainScript = gameObject.GetComponent<ZombieMain>();
    }

    void Update()
    {
        if(mainScript.moveState == ZombieMain.ZombieMoveStates.WALK || mainScript.moveState == ZombieMain.ZombieMoveStates.IDLE)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerCheckRadius, playerLayer);
            if(colliders.Length > 0)
            {
                transform.LookAt(colliders[0].gameObject.transform);
                transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0));
                StartCoroutine(DoAttack());

            }
        }
    }

    IEnumerator DoAttack()
    {
        mainScript.moveState = ZombieMain.ZombieMoveStates.ATTACK;
        mainScript.rb.isKinematic = true;
        yield return new WaitForSeconds(0.01f);
        mainScript.anim.Play("attack");
        attackSound.Play();
        mainScript.rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1f);
        if(mainScript.moveState == ZombieMain.ZombieMoveStates.ATTACK)
        {
            mainScript.rb.isKinematic = false;
            mainScript.moveState = ZombieMain.ZombieMoveStates.WALK;
        }
        
    }
}
