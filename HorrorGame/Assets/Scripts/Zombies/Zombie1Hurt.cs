using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1Hurt : MonoBehaviour, IShootable
{
    ZombieMain zombie;

    void Awake()
    {
        zombie = gameObject.GetComponent<ZombieMain>();
    }

    public void GetShot(Vector3 knockback = default(Vector3))
    {
        if(zombie.CurrentState == zombie.HurtState || zombie.CurrentState == zombie.DeadState)
            return;
            
        zombie.SwitchState(zombie.HurtState);
        
    }

    /*IEnumerator inHurt()
    {
        ZombieMain.ZombieMoveStates lastState = mainScript.moveState;

        mainScript.moveState = ZombieMain.ZombieMoveStates.HURT;
        mainScript.anim.speed = 0;
        mat.color = Color.red;
        mainScript.rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(hitStun);
        mainScript.anim.speed = 1;
        mat.color = Color.white;
        mainScript.moveState = lastState;
    }*/
}
