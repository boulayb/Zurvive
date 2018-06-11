using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ZurviveGun : VRTK_InteractableObject
{
    [Header("Gun")]
    public GameObject MagPrefab;
    public GameObject shellPrefab;
    public GameObject bulletPrefab;
    public GameObject EmptyMagPrefab;
    public GameObject ImpactEffectPrefab;
    public GameObject BloodEffectPrefab;

    public int Bullets = 10;
    public float Range = 100f;
    public bool HasMag = true;
    public float impactForce = 500f;
    public bool loaded = false;
    public bool racked = false;

    private GameObject mag;
    private GameObject trigger;
    private GameObject magSpot;
    private GameObject bulletSpot;
    private GameObject barrelEnd;
    private GameObject bullet;
    private ZurviveGunSlide slide;
    private Rigidbody slideRigidbody;
    private Collider slideCollider;
    private ParticleSystem muzzleFlash;

    private float minTriggerRotation = -10f;
    private float maxTriggerRotation = 45f;

    private VRTK_ControllerEvents controllerEvents = null;

    private void Start()
    {
        mag = transform.Find("Ammo").gameObject;
        magSpot = transform.Find("MagSpot").gameObject;
        barrelEnd = transform.Find("BarrelEnd").gameObject;
        trigger = transform.Find("Trigger").gameObject;
        bullet = transform.Find("Inside").Find("Bullet").gameObject;
        bulletSpot = transform.Find("TopPart").Find("BulletSpot").gameObject;
        slide = transform.Find("TopPart").GetComponent<ZurviveGunSlide>();
        slideRigidbody = slide.GetComponent<Rigidbody>();
        slideCollider = slide.GetComponent<Collider>();
        muzzleFlash = barrelEnd.GetComponent<ParticleSystem>();

        if (HasMag == false)
            mag.SetActive(false);
    }

    private void ToggleSlide(bool state)
    {
        if (!state)
            slide.ForceStopInteracting();
        slide.enabled = state;
        slide.isGrabbable = state;
        slideRigidbody.isKinematic = state;
        slideCollider.isTrigger = state;
    }

    public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
    {
        base.Grabbed(currentGrabbingObject);
        ToggleSlide(true);
        controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();
        if (controllerEvents != null)
        {
            controllerEvents.ButtonOnePressed += DoButtonOnePressed;
            controllerEvents.ButtonOneReleased += DoButtonOneReleased;
        }
        if (VRTK_DeviceFinder.GetControllerHand(currentGrabbingObject.controllerEvents.gameObject) == SDK_BaseController.ControllerHand.Left)
        {
            allowedTouchControllers = AllowedController.LeftOnly;
            allowedUseControllers = AllowedController.LeftOnly;
            slide.allowedGrabControllers = AllowedController.RightOnly;
        }
        else if (VRTK_DeviceFinder.GetControllerHand(currentGrabbingObject.controllerEvents.gameObject) == SDK_BaseController.ControllerHand.Right)
        {
            allowedTouchControllers = AllowedController.RightOnly;
            allowedUseControllers = AllowedController.RightOnly;
            slide.allowedGrabControllers = AllowedController.LeftOnly;
        }
    }

    public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject = null)
    {
        base.Ungrabbed(previousGrabbingObject);
        ToggleSlide(false);
        if (controllerEvents != null)
        {
            controllerEvents.ButtonOnePressed -= DoButtonOnePressed;
            controllerEvents.ButtonOneReleased -= DoButtonOneReleased;
            controllerEvents = null;
        }
        allowedTouchControllers = AllowedController.Both;
        allowedUseControllers = AllowedController.Both;
        slide.allowedGrabControllers = AllowedController.Both;
    }

    private void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        if (HasMag == true)
        {
            HasMag = false;
            magSpot.SetActive(false);
            if (Bullets > 0)
            {
                GameObject ejectedMag = Instantiate(MagPrefab, magSpot.transform.position - new Vector3(0, 0.1f, 0), magSpot.transform.rotation);
                ejectedMag.GetComponent<GunAmmo>().Bullets = Bullets;
            }
            else
                Instantiate(EmptyMagPrefab, magSpot.transform.position - new Vector3(0, 0.1f, 0), magSpot.transform.rotation);
            mag.SetActive(false);
            Bullets = 0;
        }
    }

    private void DoButtonOneReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (magSpot.activeSelf == false)
            magSpot.SetActive(true);
    }

    public override void StartUsing(VRTK_InteractUse currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        if (loaded == true && slide.IsClosed() == true)
        {
            muzzleFlash.Play();
            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.63f, 0.2f, 0.01f);
            slide.Fire();
            RaycastHit hit;
            if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.transform.forward, out hit, Range, -1, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag == Tags.head)
                {
                    GameObject impact = Instantiate(BloodEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 1.1f);
                    hit.transform.gameObject.GetComponent<ZombieDeath>().Die();
                }
                else if (hit.transform.tag == Tags.body)
                {
                    GameObject impact = Instantiate(BloodEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 1.1f);
                    hit.transform.gameObject.GetComponent<ZombieHitDetector>().Hit();
                }
                else
                {
                    GameObject impact = Instantiate(ImpactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 1.1f);
                    if (hit.rigidbody != null)
                        hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
            }
            GameObject shell = Instantiate(shellPrefab, bulletSpot.transform.position, bulletSpot.transform.rotation);
            shell.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -100f));
            loaded = false;
            if (HasMag == true && Bullets > 0)
            {
                Bullets--;
                loaded = true;
            }
            else
            {
                slide.Open();
                bullet.SetActive(false);
            }
        }
    }

    public void Load()
    {
        if (Bullets > 0)
        {
            racked = false;
            loaded = true;
            bullet.SetActive(true);
            Bullets--;
        }
        else
        {
            racked = false;
            loaded = false;
        }
    }

    public void Rack()
    {
        racked = true;
        if (loaded == true)
        {
            loaded = false;
            bullet.SetActive(false);
            GameObject shell = Instantiate(bulletPrefab, bulletSpot.transform.position, bulletSpot.transform.rotation);
            shell.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -100f));
        }
    }

    protected override void Update()
    {
        base.Update();
        if (controllerEvents)
        {
            var pressure = (maxTriggerRotation * controllerEvents.GetTriggerAxis()) - minTriggerRotation;
            trigger.transform.localEulerAngles = new Vector3(0f, 0f, -pressure);
        }
        else
            trigger.transform.localEulerAngles = new Vector3(0f, 0f, minTriggerRotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasMag == false && other.gameObject.tag == Tags.magazine)
        {
            Bullets = other.transform.parent.gameObject.GetComponent<GunAmmo>().Bullets;
            Destroy(other.transform.parent.gameObject);
            mag.SetActive(true);
            HasMag = true;
        }
    }
}
