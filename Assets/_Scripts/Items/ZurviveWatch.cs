using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZurviveWatch : MonoBehaviour
{
    public Transform Minutes;

    private float totalTime;

    private void Start()
    {
        totalTime = ZurviveTimer.instance.timeLeft;
    }

    void Update()
    {
        float minute = ZurviveTimer.instance.timeLeft;

        if (Minutes)
            Minutes.localRotation = Quaternion.Euler(0, 0, (minute / totalTime * 360) * -1);
    }
}
