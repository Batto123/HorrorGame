using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState: BaseState
{
    public override void Enter(ZombieMain zombie, BaseState lastState){}

    public override void Update(ZombieMain zombie)
    {
        zombie.anim.Play("idleZombie1");
        zombie.rigBuilder.enabled = false;

        Collider[] colliders = Physics.OverlapSphere(zombie.transform.position, zombie.attackRange, zombie.playerLayer);
        if(colliders.Length > 0)
        {
            zombie.transform.LookAt(colliders[0].gameObject.transform);
            zombie.transform.rotation = Quaternion.Euler(new Vector3(0, zombie.transform.rotation.eulerAngles.y,0));
            zombie.SwitchState(zombie.AttackState);
            return;
        }

        colliders = Physics.OverlapSphere(zombie.transform.position, zombie.viewRadius, zombie.playerLayer);
        if(colliders.Length > 0)
        {
            zombie.rigBuilder.enabled = true;
            zombie.SwitchState(zombie.WalkState);
            return;
        }
    }

    public override void Leave(ZombieMain zombie){}

    public override void OnCollisionEnter(Collision other){}
}




public class ZombieWalkState: BaseState
{
    public override void Enter(ZombieMain zombie, BaseState lastState)
    {
        zombie.mainSound.Stop();
        zombie.mainSound.Play();
    }

    public override void Update(ZombieMain zombie)
    {
        zombie.anim.Play("walkZombie1");

        Collider[] colliders = Physics.OverlapSphere(zombie.transform.position, zombie.viewRadius, zombie.playerLayer);
        if(colliders.Length == 0)
        {
            zombie.rb.velocity = Vector3.zero;

            zombie.SwitchState(zombie.IdleState);

        }else
        {
            zombie.transform.LookAt(colliders[0].gameObject.transform);
            zombie.transform.rotation = Quaternion.Euler(new Vector3(0, zombie.transform.rotation.eulerAngles.y,0));

            zombie.rb.velocity = zombie.transform.forward * zombie.walkSpeed;
        }

        colliders = Physics.OverlapSphere(zombie.transform.position, zombie.attackRange, zombie.playerLayer);
        if(colliders.Length > 0)
        {
            zombie.transform.LookAt(colliders[0].gameObject.transform);
            zombie.transform.rotation = Quaternion.Euler(new Vector3(0, zombie.transform.rotation.eulerAngles.y,0));
            zombie.SwitchState(zombie.AttackState);
        }
    }

    public override void Leave(ZombieMain zombie){}

    public override void OnCollisionEnter(Collision other){}
}
