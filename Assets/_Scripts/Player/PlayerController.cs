using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;
    public float MaxSpeedNoise = 1.8f;
    public AudioClip DeathSound;

    private AudioSource sound;
    private Vector3 lastPosition;
    private CapsuleCollider col;
    private float speed;
    private bool isDead = false;
    private bool status;
    private VRTK_HeadsetFade fader;

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
        fader = GameObject.FindGameObjectWithTag(Tags.playArea).GetComponent<VRTK_HeadsetFade>();
    }

    private void Update()
    {
        if (isDead == true && fader.IsTransitioning() == false)
            dieFallback();
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

    private void dieFallback()
    {
            PlayerStats.Energy = EnergyManager.instance.GetEnergy();
            PlayerStats.PlayerDead = status;
            SceneManager.LoadScene("MenuNextDay");
    }

    public void Die(bool Status)
    {
        if (isDead == false)
        {
            fader.Fade(Color.black, 1.5f);
            sound.Stop();
            sound.pitch = 1;
            sound.PlayOneShot(DeathSound);
            isDead = true;
            status = Status;
        }
    }
}
