using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PoolableObjectBehaviour {
    public float speedRotate = 5f;
    public float speedBoostTime = 5f;
    public Sprite icon;

    void Start()
    {
        Despawn();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speedRotate * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.GetComponent<UserInput>().SpeedBoost(speedBoostTime, icon))
            Despawn();
    }
}
