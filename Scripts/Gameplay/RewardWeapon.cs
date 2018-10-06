using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardWeapon : MonoBehaviour {
    public RewardWeapons id;
    public float speedRotate = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speedRotate * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        other.GetComponent<UserInput>().GetRewardedWep(id);
        gameObject.SetActive(false);
    }
}
