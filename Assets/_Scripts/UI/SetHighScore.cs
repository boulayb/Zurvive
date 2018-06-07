using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHighScore : MonoBehaviour
{
    private void Start()
    {
        Text text = GetComponent<Text>();
        if (PlayerPrefs.HasKey("highScore"))
            text.text = PlayerPrefs.GetInt("highScore").ToString();
        else
            text.text = "0";
    }
}
