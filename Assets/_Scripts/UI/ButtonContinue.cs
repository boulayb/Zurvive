using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonContinue : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        if (PlayerStats.PlayerDead == true || PlayerStats.Energy <= 0)
            button.transform.parent.gameObject.SetActive(false);
        else
            button.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (PlayerStats.IsDemo == true)
            SceneManager.LoadScene("ZurviveTest");
        else
            SceneManager.LoadScene("Map");
    }
}
