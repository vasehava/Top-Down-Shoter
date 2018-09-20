using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medpack : MonoBehaviour {
    public float speedRotate = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, speedRotate * Time.deltaTime, 0);
	}

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        other.GetComponent<IDamage>().Heal(50);
        gameObject.SetActive(false);
    }
}
