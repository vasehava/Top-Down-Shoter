using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShootingHandler : MonoBehaviour {

    //Add a particle and it will emit when you click the left mouse button
    public Text scoreText;
    public AudioClip shoot, reload;
    public int bullets = 30;
    public float fireRate = 0.3f;

    private float frate;
    private Animator anim;
    private Right_Stick r_stick;
    private AudioSource audioSource;


    bool fire;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        r_stick = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<Right_Stick>();
    }

	void Update () 
    {

        if (r_stick.Fire)
        {
            Shot();
        }
	}

    bool reloading;
    IEnumerator CloseReload()
    {
        reloading = true;
        audioSource.Stop();
        audioSource.PlayOneShot(reload);
        yield return new WaitForSeconds(1);
        anim.SetBool("Reload", false);
        yield return new WaitForSeconds(1.2f);
        bullets = 30;
        reloading = false;
    }

    public void Shot()
    {
        frate += Time.deltaTime;

        if (frate > fireRate)
        {
            ParticleSystem[] parts = GetComponentsInChildren<ParticleSystem>();
            //Debug.Log(parts.Length);
            if (bullets > 0)
            {
                audioSource.PlayOneShot(shoot);
                foreach (ParticleSystem ps in parts)
                {
                    //Debug.Log(ps.transform.parent.parent.name);
                    ps.Emit(1);
                }

                bullets--;
            }
            else if (!reloading)
            {
                anim.SetBool("Reload", true);
                StartCoroutine("CloseReload");
            }
            frate = 0;
            fire = false;
        }
    }
}

