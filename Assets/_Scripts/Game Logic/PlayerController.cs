using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    private Vector3 lastPosition;
    private float speed;

    private void Awake()
    {
        if (instance == null && gameObject.name == "ZurvivePlayerCollider(Clone)")
        {
            instance = this;
            this.enabled = true;
        }
        else if (gameObject.name == "ZurvivePlayerCollider")
            this.enabled = false;
        else if (instance != this)
            Destroy(gameObject);

        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        speed = Vector3.Distance(lastPosition, transform.position) / Time.deltaTime;
        lastPosition = transform.position;
    }

    public float getSpeed()
    {
        return speed;
    }
}
