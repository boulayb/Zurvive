using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZurviveTimer : MonoBehaviour
{
    public float timeLeft = 120.0f;

    public void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0.0f)
            EndOfTime();
    }

    private void EndOfTime()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
