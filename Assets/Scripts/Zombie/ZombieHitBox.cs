using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHitBox : MonoBehaviour {

    private ZombieController zombie = null;
    private bool b;
    void Start()
    {
        zombie = GetComponentInParent<ZombieController>();
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !zombie.isDead)
            other.GetComponent<IDamage>().ApplyDamage(Random.Range(zombie.minDamage, zombie.minDamage), out b);

        if (b)
            zombie.KillPlayer();
    }
}
