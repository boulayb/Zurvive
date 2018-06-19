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
    public GameObject ImpactEffectPrefab;
    public GameObject BloodEffectPrefab;
    public AudioClip FireSound;
    public AudioClip MagInSound;
    public AudioClip MagOutSound;
    public AudioClip FireEmptySound;
    public AudioClip GunLoadedSound;

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
    private GameObject soundDistance;
    private ZurviveGunSlide slide;
    private Rigidbody slideRigidbody;
    private Collider slideCollider;
    private ParticleSystem muzzleFlash;
    private AudioSource sound;

    private float minTriggerRotation = -10f;
    private float maxTriggerRotation = 45f;

    private VRTK_ControllerEvents controllerEvents = null;

    protected override void Awake()
    {
        base.Awake();
        mag = transform.Find("Ammo").gameObject;
        magSpot = transform.Find("MagSpot").gameObject;
        barrelEnd = transform.Find("BarrelEnd").gameObject;
        trigger = transform.Find("Trigger").gameObject;
        bullet = transform.Find("Inside").Find("Bullet").gameObject;
        bulletSpot = transform.Find("TopPart").Find("BulletSpot").gameObject;
        soundDistance = transform.Find("SoundDistance").gameObject;
        slide = transform.Find("TopPart").GetComponent<ZurviveGunSlide>();
        slideRigidbody = slide.GetComponent<Rigidbody>();
        slideCollider = slide.GetComponent<Collider>();
        muzzleFlash = barrelEnd.GetComponent<ParticleSystem>();
        sound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (HasMag == false)
            ToggleMag(false, 0);
        if (loaded == true)
            bullet.SetActive(true);
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
            magSpot.SetActive(false);
            Instantiate(MagPrefab, magSpot.transform.position - new Vector3(0, 0.1f, 0), magSpot.transform.rotation).GetComponent<GunAmmo>().SetBullets(Bullets);
            ToggleMag(false, 0);
            sound.PlayOneShot(MagOutSound, 1.0f);
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
            soundDistance.SetActive(true);
            muzzleFlash.Play();
            VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.63f, 0.2f, 0.01f);
            slide.Fire();
            sound.PlayOneShot(FireSound, 1.0f);
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
        else
            sound.PlayOneShot(FireEmptySound, 1.0f);
    }

    public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
    {
        base.StopUsing(previousUsingObject, resetUsingObjectState);
        soundDistance.SetActive(false);
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
        sound.PlayOneShot(GunLoadedSound, 1.0f);
    }

    public void ToggleMag(bool status, int bullets)
    {
        HasMag = status;
        mag.SetActive(status);
        Bullets = bullets;
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
            ToggleMag(true, other.transform.parent.gameObject.GetComponent<GunAmmo>().Bullets);
            Destroy(other.transform.parent.gameObject);
            sound.PlayOneShot(MagInSound, 1.0f);
        }
    }
}
