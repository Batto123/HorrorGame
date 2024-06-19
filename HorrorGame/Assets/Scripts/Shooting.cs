using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float shotDistance = 20;
    [SerializeField] private float shotPower = 10;
    [SerializeField] private float delayAfterShot = 1f;

    [SerializeField] private GameObject startingPoint;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] AudioClip shootingSound;

    [NonSerialized] private bool inDelay = false;

    private Ray bulletRay;
    private GameObject target;

    void Update()
    {
        //Wenn er gerade im Attack delay ist, soll er nicht schießen dürfen
        if(inDelay)
            return;


        //Erstellt einen RayCast, also eine unsichtbare Linie, der die Flugbahn von der bullet nach treffern Checken soll
        bulletRay = new Ray(startingPoint.transform.position, startingPoint.transform.forward);

        bool hasShot = Input.GetKeyDown(KeyCode.Mouse0);

        //In hitinfo werden später die Daten gespeichert, die der bulletRay bekommt, wenn er ein gameObject trifft
        RaycastHit hitInfo;
        if(hasShot)
        {
            //Wird aufgerufen, wenn ein Objekt getroffen wird
            if(Physics.Raycast(bulletRay, out hitInfo, shotDistance, hitLayers))
            {
                target = hitInfo.collider.gameObject;

                //Wenn das Objekt IShootable unterstützt, wird GetShot von IShootable aufgerufen
                if(target.TryGetComponent<IShootable>(out IShootable shootable))
                    shootable.GetShot(startingPoint.transform.forward * shotPower);
            }

            //Alles hier macht er immer, egal ob etwas getroffen wird oder nicht
            AudioSource.PlayClipAtPoint(shootingSound, transform.position);
            StartCoroutine(waitAfterShot());
        }
        
    }

    //Aktiviert für eine halbe Sekunde Attack delay
    IEnumerator waitAfterShot()
    {
        inDelay = true;
        yield return new WaitForSeconds(delayAfterShot);
        inDelay = false;
    }
}
