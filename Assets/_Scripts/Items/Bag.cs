using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public AudioClip BagInSound;

    private AudioSource sound;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.food)
        {
            EnergyManager.instance.AddEnergy(EnergyManager.instance.energyGainedFood);
            Destroy(other.transform.parent.gameObject);
            sound.PlayOneShot(BagInSound, 1.0f);
        }
    }
}