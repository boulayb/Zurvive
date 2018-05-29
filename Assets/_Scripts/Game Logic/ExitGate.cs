using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerController.instance.gameObject)
        {
            EnergyManager.instance.LooseEnergy(EnergyManager.EnergyEventName.NEXTDAY);
            PlayerStats.Energy = EnergyManager.instance.GetEnergy();
            PlayerStats.DaysSurvived += 1;
            SceneManager.LoadScene("MenuNextDay");
        }
    }
}
