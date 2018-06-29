using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class FadeOnStart : MonoBehaviour
{
    public Color FadeColor = Color.black;
    public float TimeToFade = 2f;

	void Start ()
    {
        GetComponent<VRTK_HeadsetFade>().Fade(FadeColor, TimeToFade);
	}
	
}
