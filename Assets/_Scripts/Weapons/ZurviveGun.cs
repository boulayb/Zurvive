using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ZurviveGun : VRTK_InteractableObject
{
    public GameObject MagPrefab;
    public GameObject EmptyMagPrefab;
    public int Bullets = 10;
    public bool HasMag = true;

    private GameObject mag;
    private GameObject magTrigger;

    private void Start()
    {
        mag = gameObject.transform.GetChild(0).gameObject;
        magTrigger = gameObject.transform.GetChild(7).gameObject;

        if (HasMag == false)
            mag.SetActive(false);
    }

    public override void StartUsing(VRTK_InteractUse currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        if (HasMag == true)
        {
            HasMag = false;
            magTrigger.SetActive(false);
            if (Bullets > 0)
            {
                GameObject ejectedMag = Instantiate(MagPrefab, magTrigger.transform.position - new Vector3(0, (float)0.1, 0), magTrigger.transform.rotation);
                ejectedMag.GetComponent<GunAmmo>().Bullets = Bullets;
            }
            else
                Instantiate(EmptyMagPrefab, magTrigger.transform.position - new Vector3(0, (float)0.1, 0), magTrigger.transform.rotation);
            mag.SetActive(false);
            Bullets = 0;
        }
    }

    public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
    {
        base.StopUsing(previousUsingObject, resetUsingObjectState);
        if (magTrigger.activeSelf == false)
            magTrigger.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasMag == false && other.gameObject.tag == Tags.magazine)
        {
            Bullets = other.gameObject.transform.parent.gameObject.GetComponent<GunAmmo>().Bullets;
            Destroy(other.gameObject.transform.parent.gameObject);
            mag.SetActive(true);
            HasMag = true;
        }
    }
}
