using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.food)
        {
            EnergyManager.instance.AddEnergy(EnergyManager.instance.energyGainedFood);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}