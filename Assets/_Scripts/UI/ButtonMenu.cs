using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (PlayerPrefs.HasKey("highScore") == false || PlayerPrefs.GetInt("highScore") < PlayerStats.DaysSurvived)
            PlayerPrefs.SetInt("highScore", PlayerStats.DaysSurvived);
        SceneManager.LoadScene("MenuMain");
    }
}
