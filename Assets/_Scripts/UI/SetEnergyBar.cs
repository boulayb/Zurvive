using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetEnergyBar : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().fillAmount = PlayerStats.Energy / PlayerStats.MaxEnergy;
    }
}
