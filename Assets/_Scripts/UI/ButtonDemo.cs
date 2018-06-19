using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonDemo : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        PlayerStats.IsDemo = true;
        PlayerStats.DaysSurvived = 0;
        PlayerStats.PlayerDead = false;
        PlayerStats.MaxEnergy = -1;
        PlayerStats.Energy = -1;
        PlayerStats.Bullets1 = -1;
        PlayerStats.Bullets2 = -1;
        PlayerStats.Bullets3 = -1;
        PlayerStats.BulletsGun = 10;
        PlayerStats.Gun = true;
        PlayerStats.Melee = true;
        SceneManager.LoadScene("ZurviveTest");
    }
}
