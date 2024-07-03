using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState: BaseState
{
    public override void Enter(ZombieMain zombie, BaseState lastState)
    {
        
        zombie.rb.isKinematic = true;
        zombie.mainSound.Stop();
        zombie.attackSound.Play();
        zombie.rb.velocity = Vector3.zero;
        activeTimer = zombie.StartTimer(1.5f, Attackfinished);
        timerActive = true;
    }

    public override void Update(ZombieMain zombie)
    {
        zombie.anim.Play("attack");
    }

    public override void Leave(ZombieMain zombie)
    {
        if(timerActive)
        {
            zombie.CancelTimer(activeTimer);
            Attackfinished(zombie);
        }
    }

    public override void OnCollisionEnter(Collision other){}

    public void Attackfinished(ZombieMain zombie)
    {
        timerActive = false;
        zombie.rb.isKinematic = false;
        zombie.SwitchState(zombie.IdleState);
        activeTimer = null;
    }
}
