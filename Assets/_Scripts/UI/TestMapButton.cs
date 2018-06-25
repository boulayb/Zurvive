using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestMapButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        if (PlayerStats.IsDemo == false)
            transform.parent.gameObject.SetActive(false);
    }

    private void TaskOnClick()
    {
        PlayerStats.PlayerDead = false;
        PlayerStats.MaxEnergy = -1;
        PlayerStats.Energy = -1;
        PlayerStats.Bullets1 = -1;
        PlayerStats.Bullets2 = -1;
        PlayerStats.Bullets3 = -1;
        PlayerStats.BulletsGun = 1;
        PlayerStats.Gun = true;
        PlayerStats.Melee = true;
        SceneManager.LoadScene("MapTest");
    }
}
