using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void Shooting();
public class Weapon : MonoBehaviour {

    public string weaponName;
    public string weaponType;
    public float fireRate;
    public int bullets;
    public float reloadTime = 2f;

    public Sprite weaponIcon;
    public AudioClip shoot, reload;
    public ParticleSystem[] parts;
    public GameObject leftHandWeapon = null;
    private AudioSource audioSource;
    private Weapon leftWeapon = null;
    private Animator anim;
    private float frate;
    private bool reloading, fire;

    private bool leftWep, rightWep;

    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        if (leftHandWeapon != null)
        {
            leftWeapon = leftHandWeapon.GetComponent<Weapon>();
            leftWep = false;
            rightWep = true;
        }
    }
    public void SetUpGun(Animator anim)
    {
        this.anim = anim;
        switch (weaponType)
        {
            case "Gun":
                anim.SetInteger("WeaponID", 0);
                break;
            case "TwoPistols":
                anim.SetInteger("WeaponID", 1);
                break;
            default:
                break;
        }

        gameObject.SetActive(true);
        if (leftHandWeapon != null)
            leftHandWeapon.SetActive(true);
    }
    public void RemoveGun()
    {
        gameObject.SetActive(false);
        if (leftHandWeapon != null)
            leftHandWeapon.SetActive(false);
    }

    public void Shot()
    {
        frate += Time.deltaTime;

        if (frate > fireRate)
        {
            if (bullets > 0)
            {
                audioSource.PlayOneShot(shoot);
                foreach (ParticleSystem ps in parts)
                {
                    ps.Emit(1);
                }

                //bullets--;
            }
            else if (!reloading)
            {
                //anim.SetBool("Reload", true);
                StartCoroutine("CloseReload");
            }
            frate = 0;
            fire = false;
        }
    }
    private void LeftHandShot()
    {
        leftWeapon.frate += Time.deltaTime;

        if (leftWeapon.frate > leftWeapon.fireRate)
        {

            if (leftWeapon.bullets > 0)
            {
                audioSource.PlayOneShot(shoot);
                foreach (ParticleSystem ps in leftWeapon.parts)
                {
                    ps.Emit(1);
                }

                //leftWeapon.bullets--;
            }
            else if (!reloading)
            {
                //anim.SetBool("Reload", true);
                StartCoroutine("CloseReload");
            }
            leftWeapon.frate = 0;
            leftWeapon.fire = false;
            rightWep = true;
            leftWep = false;
        }
    }
    public void TwoHandsShot()
    {
        if (rightWep)
        {
            frate += Time.deltaTime;

            if (frate > fireRate)
            {

                if (bullets > 0)
                {
                    audioSource.PlayOneShot(shoot);
                    foreach (ParticleSystem ps in parts)
                    {
                        Debug.Log(ps.transform.parent.parent.name);
                        ps.Emit(1);
                    }

                    //bullets--;
                }
                else if (!reloading)
                {
                    LeftHandShot();
                    return;
                }
                frate = 0;
                fire = false;
                rightWep = false;
                leftWep = true;
            }
        }
        else
        {
            LeftHandShot();
        }
    }

    public Shooting GetShooting
    {
        get
        {
            switch (weaponType)
            {
                case "Gun":
                    return Shot;
                case "TwoPistols":
                    return TwoHandsShot;
                default:
                    return Shot;
            }

        }
    }
    IEnumerator CloseReload()
    {
        reloading = true;
        GetComponentInParent<UserInput>().reloadUI.Reloading(reloadTime);
        print("Close relaod");
        audioSource.Stop();
        audioSource.PlayOneShot(reload);
        yield return new WaitForSeconds(reloadTime);
        //anim.SetBool("Reload", false);
        //yield return new WaitForSeconds(1.2f);
        if (leftWeapon != null)
        {
            bullets = 8;
            leftWeapon.bullets = 8;
        }
        else bullets = 30;

        reloading = false;
    }
}
