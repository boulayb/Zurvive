using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMenuTitle : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        if (PlayerStats.Energy <= 0)
            text.text = "YOU STARVED TO DEATH";
        else if (PlayerStats.PlayerDead == true)
            text.text = "YOU DIED";
        else
            text.text = "YOU SURVIVED THIS DAY";
    }
}
