﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GunAmmo : VRTK_InteractableObject
{
    public GameObject BulletPrefab;
    public int Bullets = 10;
    public int MaxBullets = 10;

    private GameObject bullet;
    private GameObject trigger;

    protected override void Awake()
    {
        base.Awake();
        bullet = transform.Find("Bullet").gameObject;
        trigger = transform.Find("Capsule").gameObject;
    }

    private void Start()
    {
        if (Bullets <= 0)
            bullet.SetActive(false);
    }

    public override void StartUsing(VRTK_InteractUse currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        if (Bullets > 0)
        {
            Bullets--;
            trigger.SetActive(false);
            Instantiate(BulletPrefab, bullet.transform.position, bullet.transform.rotation);
            if (Bullets <= 0)
                bullet.SetActive(false);
        }
    }

    public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
    {
        base.StopUsing(previousUsingObject, resetUsingObjectState);
        if (trigger.activeSelf == false)
            trigger.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Bullets < MaxBullets && other.gameObject.tag == Tags.bullet)
        {
            Bullets++;
            Destroy(other.gameObject);
            if (Bullets == 1 && bullet != null)
                bullet.SetActive(true);
        }
    }

    public void SetBullets(int bullets)
    {
        Bullets = bullets;
        if (Bullets <= 0)
            bullet.SetActive(false);
        else
            bullet.SetActive(true);
    }
}
