using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetIK : MonoBehaviour
{
    [SerializeField] private GameObject controllingFoot;
    ZombieMain main;

    void Update()
    {
        transform.position = new Vector3(controllingFoot.transform.position.x, controllingFoot.transform.position.y, controllingFoot.transform.position.z);
        if (Physics.Raycast(transform.position + (Vector3.up * 1), Vector3.down, out RaycastHit groundHit, 2f))
        {
            float angle = Vector3.Angle(Vector3.up, groundHit.normal);
            transform.rotation = Quaternion.Euler(groundHit.normal);
            transform.position = new Vector3(controllingFoot.transform.position.x, groundHit.point.y + 0.1f, controllingFoot.transform.position.z);
        }
    }

}
