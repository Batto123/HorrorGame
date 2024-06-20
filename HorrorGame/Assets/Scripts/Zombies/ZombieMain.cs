using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMain : MonoBehaviour
{
    public enum ZombieMoveStates
    {
        IDLE,
        WALK,
        ATTACK,
        HURT,
        DEAD
    }

    public ZombieMoveStates moveState = ZombieMoveStates.IDLE;
}
