using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HipStorage : MonoBehaviour
{
    public static HipStorage instance = null;

    public GameObject GunPrefab;
    public GameObject MeleePrefab;
    public GameObject AmmoPrefab;

    private VRTK_SnapDropZone holsterRight;
    private VRTK_SnapDropZone holsterLeft;
    private VRTK_SnapDropZone holsterMiddleLeft;
    private VRTK_SnapDropZone holsterMiddleRight;
    private VRTK_SnapDropZone holsterMiddle;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        holsterRight = transform.Find("Holster Right").Find("Holster_SnapDropZone").GetComponent<VRTK_SnapDropZone>();
        holsterLeft = transform.Find("Holster Left").Find("Holster_SnapDropZone").GetComponent<VRTK_SnapDropZone>();
        holsterMiddleRight = transform.Find("Holster Middle Right").Find("Holster_SnapDropZone").GetComponent<VRTK_SnapDropZone>();
        holsterMiddleLeft = transform.Find("Holster Middle Left").Find("Holster_SnapDropZone").GetComponent<VRTK_SnapDropZone>();
        holsterMiddle = transform.Find("Holster Middle").Find("Holster_SnapDropZone").GetComponent<VRTK_SnapDropZone>();
    }

    public GameObject GetGun()
    {
        return (holsterRight.GetCurrentSnappedObject());
    }

    public GameObject GetMelee()
    {
        return (holsterLeft.GetCurrentSnappedObject());
    }

    public GameObject GetAmmo1()
    {
        return (holsterMiddleLeft.GetCurrentSnappedObject());
    }

    public GameObject GetAmmo2()
    {
        return (holsterMiddle.GetCurrentSnappedObject());
    }

    public GameObject GetAmmo3()
    {
        return (holsterMiddleRight.GetCurrentSnappedObject());
    }

    public void AttachGun(int bullets)
    {
        GameObject gun = Instantiate(GunPrefab, gameObject.transform.position, gameObject.transform.rotation);
        ZurviveGun script = gun.GetComponent<ZurviveGun>();
        if (bullets == -1)
            script.ToggleMag(false, 0);
        else
            script.ToggleMag(true, bullets);
        holsterRight.ForceSnap(gun);
    }

    public void AttachMelee()
    {
        holsterLeft.ForceSnap(Instantiate(MeleePrefab, gameObject.transform.position, gameObject.transform.rotation));
    }

    public void AttachAmmo1(int bullets)
    {
        GameObject ammo = Instantiate(AmmoPrefab, gameObject.transform.position, gameObject.transform.rotation);
        ammo.GetComponent<GunAmmo>().SetBullets(bullets);
        holsterMiddleLeft.ForceSnap(ammo);
    }

    public void AttachAmmo2(int bullets)
    {
        GameObject ammo = Instantiate(AmmoPrefab, gameObject.transform.position, gameObject.transform.rotation);
        ammo.GetComponent<GunAmmo>().SetBullets(bullets);
        holsterMiddle.ForceSnap(ammo);
    }

    public void AttachAmmo3(int bullets)
    {
        GameObject ammo = Instantiate(AmmoPrefab, gameObject.transform.position, gameObject.transform.rotation);
        ammo.GetComponent<GunAmmo>().SetBullets(bullets);
        holsterMiddleRight.ForceSnap(ammo);
    }
}
