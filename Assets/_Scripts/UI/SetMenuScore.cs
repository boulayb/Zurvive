﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMenuScore : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Text>().text = PlayerStats.DaysSurvived.ToString();
    }
}
