using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonNewGame : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        PlayerStats.DaysSurvived = 0;
        PlayerStats.PlayerDead = false;
        PlayerStats.MaxEnergy = 1;
        PlayerStats.Energy = 1;
        SceneManager.LoadScene("ZurviveTest");
    }
}
