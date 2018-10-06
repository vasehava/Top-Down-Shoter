using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medpack : PoolableObjectBehaviour {
    public float speedRotate = 5f;
    void Start () {

        Despawn();
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
        Despawn();
    }
    
}
