using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ZurviveGunSlide : VRTK_InteractableObject
{
    private float restPosition;
    private float fireTimer = 0f;
    private float fireDistance = 0.13f;
    private float boltSpeed = 0.04f;
    private bool locked = false;
    private bool closed = true;
    private ZurviveGun gun;

    public void Start()
    {
        gun = transform.parent.GetComponent<ZurviveGun>();
    }

    public void Fire()
    {
        fireTimer = fireDistance;
    }

    public void Open()
    {
        transform.localPosition = new Vector3(restPosition - (fireDistance * 2), transform.localPosition.y, transform.localPosition.z);
        locked = true;
    }

    public bool IsClosed()
    {
        return (closed);
    }

    public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
    {
        base.Grabbed(currentGrabbingObject);
        locked = false;
    }

    protected override void Awake()
    {
        base.Awake();
        restPosition = transform.localPosition.x;
    }

    protected override void Update()
    {
        base.Update();
        if (transform.localPosition.x >= restPosition && locked == false)
        {
            transform.localPosition = new Vector3(restPosition, transform.localPosition.y, transform.localPosition.z);
        }

        if (fireTimer == 0 && transform.localPosition.x < restPosition && !IsGrabbed() && locked == false)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + boltSpeed, transform.localPosition.y, transform.localPosition.z);
        }

        if (fireTimer > 0 && locked == false)
        {
            transform.localPosition = new Vector3(transform.localPosition.x - boltSpeed, transform.localPosition.y, transform.localPosition.z);
            fireTimer -= boltSpeed;
        }

        if (fireTimer < 0 || locked == false)
        {
            fireTimer = 0;
        }

        if (transform.localPosition.x >= restPosition)
        {
            closed = true;
            if (gun.racked == true)
                gun.Load();
        }
        else if (transform.localPosition.x <= restPosition - (fireDistance * 2))
        {
            closed = false;
            gun.Rack();
        }
        else
            closed = false;
    }
}
