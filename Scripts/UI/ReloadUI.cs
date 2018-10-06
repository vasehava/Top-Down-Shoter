using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReloadUI : MonoBehaviour {
    private Image reload_img;
    private bool reload;
    private float timer = 0f;
    private float reloadTime = 0f;
	// Use this for initialization
	void Start () {
        reload_img = GetComponent<Image>();
	}
	
    void Update()
    {
        if(reload)
        {
            timer += Time.deltaTime;
            reload_img.fillAmount = timer / reloadTime;
        }
        if(timer >= reloadTime)
        {
            reload = false;
            timer = 0f;
            reload_img.fillAmount = 0f;
        }
    }
	public void Reloading(float reloadTime)
    {
        reload = true;
        this.reloadTime = reloadTime;
    }

    private IEnumerator Rel(float reloadTime)
    {
        reload_img.fillAmount = 0.0f;
        for (int i = 0; i <= 50; i++)
        {
            yield return new WaitForSeconds(reloadTime / 50);
            reload_img.fillAmount += 0.02f;
        }

        reload_img.fillAmount = 0.0f;
    }
}
