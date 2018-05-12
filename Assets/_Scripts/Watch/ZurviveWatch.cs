using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZurviveWatch : MonoBehaviour
{
    public Transform Minutes;

    private ZurviveTimer timer;
    private float totalTime;

    private void Awake()
    {
        timer = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<ZurviveTimer>();
        totalTime = timer.timeLeft;
    }

    void Update()
    {
        float minute = timer.timeLeft;

        if (Minutes)
            Minutes.localRotation = Quaternion.Euler(0, 0, (minute / totalTime * 360) * -1);
    }
}
