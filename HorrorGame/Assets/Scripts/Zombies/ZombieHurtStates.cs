using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieHurtState: BaseState
{

    public override void Enter(ZombieMain zombie, BaseState lastState)
    {
        zombie.currentLifes--;

        if(zombie.currentLifes < 1)
        {
            zombie.SwitchState(zombie.DeadState);
            return;
        }

        zombie.anim.speed = 0;
        zombie.material.color = Color.red;
        zombie.rb.velocity = Vector3.zero;
        zombie.StartTimer(zombie.hitStun, SufferingFinished);
        timerActive = true;
    }

    public override void Update(ZombieMain zombie){}

    public override void Leave(ZombieMain zombie)
    {
        if(timerActive)
        {
            zombie.CancelTimer(activeTimer);
            SufferingFinished(zombie);
        }
    }

    public override void OnCollisionEnter(Collision other){}

    public void SufferingFinished(ZombieMain zombie)
    {
        timerActive = false;
        zombie.anim.speed = 1;
        zombie.material.color = Color.white;
        zombie.SwitchState(zombie.IdleState);
    }
}

public class ZombieDeadState: BaseState
{
    public override void Enter(ZombieMain zombie, BaseState lastState)
    {
        zombie.anim.enabled = false;
        zombie.rc.ActivateRagdoll();
        zombie.rb.velocity = Vector3.zero;
        zombie.rb.isKinematic = true;
        zombie.GetComponent<Collider>().enabled = false;
    }

    public override void Update(ZombieMain zombie){}

    public override void Leave(ZombieMain zombie){}

    public override void OnCollisionEnter(Collision other){}
}
