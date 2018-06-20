using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    private Vector3 lastPosition;
    private CapsuleCollider col;
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

        col = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        speed = Vector3.Distance(lastPosition, transform.position) / Time.deltaTime;
        lastPosition = transform.position;
    }

    public float getHeight()
    {
        return col.height;
    }

    public CapsuleCollider getCollider()
    {
        return col;
    }

    public float getSpeed()
    {
        return speed;
    }
}
