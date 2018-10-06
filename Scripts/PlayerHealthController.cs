using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour, IDamage {

    public int maxHP = 100;
    [HideInInspector]
    public Game_Manager gameManager;
    public GameObject ragdoll;
    public Image bloodyScreen;
    private Image damageScreen;
    private int currentHP;
    private Slider sliderHealthBar;
    private Animator sliderHealthBarAnim;
    void Start()
    {
        currentHP = maxHP;
        GameObject gObj = GameObject.Find("GameplayCanvas");
        gameManager = GameObject.FindObjectOfType<Game_Manager>();
        sliderHealthBar = gObj.GetComponentInChildren<Slider>();
        sliderHealthBarAnim = gObj.GetComponentInChildren<Animator>();
        Image[] imgs = gObj.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < imgs.Length; i++)
        {
            if (imgs[i].name == "BloodScreen")
            {
                bloodyScreen = imgs[i];
                continue;
            }
            if (imgs[i].name == "DamageScreen")
            {
                damageScreen = imgs[i];
                damageScreen.gameObject.SetActive(false);
                continue;
            }
        }

        sliderHealthBar.maxValue = maxHP;
        UpdateHealthBar();
    }

    public void ApplyDamage(int damage, out bool killed)
    {
        killed = false;
        currentHP -= damage;
        currentHP = currentHP <= 0 ? 0 : currentHP;
        StartCoroutine(ActivateDamageScreen());
        UpdateHealthBar();
        if (currentHP <= 0)
            Die();
    }
    private IEnumerator ActivateDamageScreen()
    {
        damageScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        damageScreen.gameObject.SetActive(false);

    }
    public void Die()
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        damageScreen.gameObject.SetActive(false);
        bloodyScreen.gameObject.SetActive(true);
        //gameManager.StopAllCoroutines();
        gameManager.StopGenerateZombie();
        Destroy(gameObject);
    }

    public void Heal(int heal)
    {
        if (currentHP == maxHP)
            return;

        int fact_heal = maxHP - currentHP < heal ? maxHP - currentHP : heal;

        if(((float)currentHP + fact_heal) / maxHP > 0.2f)
            sliderHealthBarAnim.SetBool("LowHP", false);

        StartCoroutine(Healing(fact_heal));
        currentHP += fact_heal;
        //UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        sliderHealthBar.value = currentHP;

        if ((float)currentHP / maxHP <= 0.2f)
            sliderHealthBarAnim.SetBool("LowHP", true);
        else
            sliderHealthBarAnim.SetBool("LowHP", false);
    }

    IEnumerator Healing(int heal)
    {
        int c = heal;
        while(heal >= 0)
        {
            yield return new WaitForSeconds((float)3/c);
            sliderHealthBar.value++;
            heal--;
        }
    }

    public void ApplyDamage(int damage, out bool killed, out GameObject obj)
    {
        throw new System.NotImplementedException();
    }
}
