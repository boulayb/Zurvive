using UnityEngine;

public class ZurviveTimer : MonoBehaviour
{
    public static ZurviveTimer instance = null;

    public float timeLeft = 120.0f;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0.0f)
            EndOfTime();
    }

    private void EndOfTime()
    {
        PlayerController.instance.Die(true);
    }
}
