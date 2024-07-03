using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected bool timerActive = false;
    protected Coroutine activeTimer;

    public abstract void Enter(ZombieMain zombie, BaseState lastState);

    public abstract void Update(ZombieMain zombie);

    public abstract void Leave(ZombieMain zombie);

    public abstract void OnCollisionEnter(Collision other);
}
