using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VRTK;

public class MenuFix : MonoBehaviour
{
    public GameObject EventSystem;
    public VRTK_UIPointer PointerController;

    void Start()
    {
        StartCoroutine(LateStart(1));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EventSystem.SetActive(true);
        EventSystem.GetComponent<EventSystem>().enabled = false;
        List<VRTK_UIPointer> pointers = EventSystem.GetComponent<VRTK_VRInputModule>().pointers;
        if (pointers.Count == 0)
            pointers.Add(PointerController);
    }
}
