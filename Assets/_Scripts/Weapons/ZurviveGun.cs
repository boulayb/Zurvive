using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ZurviveGun : VRTK_InteractableObject
{
    [Header("Gun")]
    public GameObject MagPrefab;
    public GameObject EmptyMagPrefab;
    public int Bullets = 10;
    public float Range = 100f;
    public bool HasMag = true;

    private GameObject mag;
    private GameObject magTrigger;
    private GameObject barrelEnd;
    private ParticleSystem muzzleFlash;

    private VRTK_ControllerEvents controllerEvents = null;
    private GameObject leftController;
    private GameObject rightController;

    private void Start()
    {
        mag = gameObject.transform.GetChild(0).gameObject;
        magTrigger = gameObject.transform.GetChild(7).gameObject;
        barrelEnd = gameObject.transform.GetChild(8).gameObject;
        muzzleFlash = barrelEnd.GetComponent<ParticleSystem>();

        if (HasMag == false)
            mag.SetActive(false);
    }

    public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
    {
        base.Grabbed(currentGrabbingObject);
        if (leftController == null || rightController == null)
        {
            leftController = GameObject.FindWithTag(Tags.leftController);
            rightController = GameObject.FindWithTag(Tags.rightController);
        }
        if (leftController.GetComponent<VRTK_InteractGrab>() == currentGrabbingObject)
            controllerEvents = leftController.GetComponent<VRTK_ControllerEvents>();
        else if (rightController.GetComponent<VRTK_InteractGrab>() == currentGrabbingObject)
            controllerEvents = rightController.GetComponent<VRTK_ControllerEvents>();
        if (controllerEvents != null)
        {
            controllerEvents.ButtonOnePressed += DoButtonOnePressed;
            controllerEvents.ButtonOneReleased += DoButtonOneReleased;
        }
    }

    public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject = null)
    {
        base.Ungrabbed(previousGrabbingObject);
        if (controllerEvents != null)
        {
            controllerEvents.ButtonOnePressed -= DoButtonOnePressed;
            controllerEvents.ButtonOneReleased -= DoButtonOneReleased;
            controllerEvents = null;
        }
    }

    private void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        if (HasMag == true)
        {
            HasMag = false;
            magTrigger.SetActive(false);
            if (Bullets > 0)
            {
                GameObject ejectedMag = Instantiate(MagPrefab, magTrigger.transform.position - new Vector3(0, 0.1f, 0), magTrigger.transform.rotation);
                ejectedMag.GetComponent<GunAmmo>().Bullets = Bullets;
            }
            else
                Instantiate(EmptyMagPrefab, magTrigger.transform.position - new Vector3(0, 0.1f, 0), magTrigger.transform.rotation);
            mag.SetActive(false);
            Bullets = 0;
        }
    }

    private void DoButtonOneReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (magTrigger.activeSelf == false)
            magTrigger.SetActive(true);
    }


    public override void StartUsing(VRTK_InteractUse currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        if (HasMag == true && Bullets > 0)
        {
            muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.transform.forward, out hit, Range, -1, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag == Tags.head)
                    hit.transform.gameObject.GetComponent<ZombieDeath>().Die();
                else if (hit.transform.tag == Tags.body)
                    hit.transform.gameObject.GetComponent<ZombieHitDetector>().Hit();
            }
            Bullets--;
        }
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
