using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1Hurt : MonoBehaviour, IShootable
{
    Rigidbody rb;
    Material mat;
    ZombieMain mainScript;

    [SerializeField] int maxLifes = 3;
    [SerializeField] float hitStun = 0.2f;

    private int currentLifes;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        mainScript = gameObject.GetComponent<ZombieMain>();
        mat = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;

        currentLifes = maxLifes;
    }

    public void GetShot(Vector3 knockback = default(Vector3))
    {
        if(mainScript.moveState == ZombieMain.ZombieMoveStates.HURT)
            return;

        if(currentLifes <= 1)
            Destroy(gameObject);

        currentLifes--;
        StartCoroutine(inHurt());
    }

    IEnumerator inHurt()
    {
        mainScript.moveState = ZombieMain.ZombieMoveStates.HURT;
        mat.color = Color.red;
        Debug.Log("AAAAH");
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(hitStun);
        mat.color = Color.white;
        mainScript.moveState = ZombieMain.ZombieMoveStates.IDLE;
    }
}
