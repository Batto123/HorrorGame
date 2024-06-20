using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] GameObject Hips;
    [SerializeField] GameObject Spine;
    [SerializeField] GameObject Neck;
    [SerializeField] GameObject UpperLegR;
    [SerializeField] GameObject LowerLegR;
    [SerializeField] GameObject UpperLegL;
    [SerializeField] GameObject LowerLegL;
    [SerializeField] GameObject ShoulderR;
    [SerializeField] GameObject ElbowR;
    [SerializeField] GameObject ShoulderL;
    [SerializeField] GameObject ElbowL;

    private GameObject[] bodyParts;

    void Awake()
    {
        bodyParts = new GameObject[] {Hips, Spine, Neck, UpperLegR, LowerLegR, UpperLegL, LowerLegL, ShoulderR, ElbowR, ShoulderL, ElbowL};

        foreach(GameObject part in bodyParts)
        {
            part.GetComponent<RagdollPart>().mainParent = this.gameObject;
            part.GetComponent<Rigidbody>().isKinematic = true;
            part.GetComponent<Collider>().enabled = false;
        }

        DeactivateRagdoll();
    }

    public void ActivateRagdoll()
    {
        foreach(GameObject part in bodyParts)
        {
            part.GetComponent<Rigidbody>().isKinematic = false;
            part.GetComponent<Collider>().enabled = true;
        }
    }

    public void DeactivateRagdoll()
    {
        foreach(GameObject part in bodyParts)
        {
            part.GetComponent<Rigidbody>().isKinematic = true;
            part.GetComponent<Collider>().enabled = false;
        }
    }
}
