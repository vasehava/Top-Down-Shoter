using UnityEngine;
using System.Collections;

public class DestroyOverTime : PoolableObjectBehaviour, IPoolable {
    //public bool isSmoke = true;
    //public event Despawn OnDespawnEvent;
    //public event Spawn OnSpawnEvent;

    public float timeToDestroy = 1.5f;

	void Start () 
    {
        Invoke("Despawn", timeToDestroy);
	}
	
    void OnEnable()
    {
        Invoke("Despawn", timeToDestroy);
    }

    //public void Despawn()
    //{
    //    if (isSmoke) OnDespawnEvent.Invoke(PoolType.SmokeParticles, gameObject);
    //    else OnDespawnEvent.Invoke(PoolType.BloodParticles, gameObject);
    //}
    //public void Spawn(PoolType id, GameObject prefab, Vector3 pos, Quaternion rot)
    //{
    //    OnSpawnEvent.Invoke(id, prefab, pos, rot);
    //}

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {

        //if (isSmoke) OnDespawnEvent.Invoke(PoolType.SmokeParticles, gameObject);
        //else OnDespawnEvent.Invoke(PoolType.BloodParticles, gameObject);
    }
}
