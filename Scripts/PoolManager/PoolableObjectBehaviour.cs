using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObjectBehaviour : MonoBehaviour{

    public PoolType poolID;
    public event Despawn OnDespawnEvent;
    public event Spawn OnSpawnEvent;

    public void Spawn(PoolType id, GameObject prefab, Vector3 pos, Quaternion rot)
    {
        OnSpawnEvent.Invoke(id, prefab, pos, rot);
    }
    public void Despawn()
    {
        OnDespawnEvent.Invoke(poolID, gameObject);
    }

}
