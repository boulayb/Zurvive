using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;
    public float MaxSpeedNoise = 1.8f;

    private AudioSource sound;
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
        sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (speed >= MaxSpeedNoise && sound.isPlaying == false)
        {
            if (speed >= 2.1f)
                sound.pitch = 3f;
            else
                sound.pitch = 2f;
            sound.Play();
        }
        else if (sound.isPlaying == false)
            sound.Stop();
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
