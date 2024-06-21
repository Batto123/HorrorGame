using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1Hurt : MonoBehaviour, IShootable
{
    ZombieMain mainScript;
    Material mat;

    [SerializeField] int maxLifes = 3;
    [SerializeField] float hitStun = 0.2f;

    private int currentLifes;

    void Awake()
    {
        mainScript = gameObject.GetComponent<ZombieMain>();
        mat = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;

        currentLifes = maxLifes;
    }

    public void GetShot(Vector3 knockback = default(Vector3))
    {
        if(mainScript.moveState == ZombieMain.ZombieMoveStates.HURT)
            return;

        if(currentLifes <= 1)
        {
            mainScript.moveState = ZombieMain.ZombieMoveStates.DEAD;
            mainScript.anim.enabled = false;
            mainScript.rc.ActivateRagdoll();
            mainScript.rb.velocity = Vector3.zero;
            mainScript.rb.isKinematic = true;
            gameObject.GetComponent<Collider>().enabled = false;
        }

        if(mainScript.moveState != ZombieMain.ZombieMoveStates.DEAD)
        {
            currentLifes--;
            StartCoroutine(inHurt());
        }
        
    }

    IEnumerator inHurt()
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
    }
}
