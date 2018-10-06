using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Despawn(PoolType id, GameObject obj);
public delegate GameObject Spawn(PoolType id, GameObject prefab, Vector3 position = default(Vector3),
        Quaternion rotation = default(Quaternion),
        Transform parent = null);
public class Pool : MonoBehaviour{

    private Transform parentPool;
    private Dictionary<int, Stack<GameObject>> cachedObjects = new Dictionary<int, Stack<GameObject>>();
    private Dictionary<int, int> cachedIds = new Dictionary<int, int>();
    //protected int index;

    public Pool PopulateWith(GameObject prefab, int amount, int amountPerTick, int tickSize = 1)
    {
        var key = prefab.GetInstanceID();
        Stack<GameObject> stack;
        var stacked = cachedObjects.TryGetValue(key, out stack);
        if (stacked == false)
            cachedObjects.Add(key, new Stack<GameObject>());

        Populating(prefab, amount, amountPerTick, key);
        
        return this;
    }

    private IEnumerator Populating(GameObject prefab, int amount, int amountPerTick, int key)
    {
        while (amount > 0)
        {
            yield return new WaitForEndOfFrame();
            for(int i = 0; i < amountPerTick; i++)
            {
                var go = Populate(prefab, Vector3.zero, Quaternion.identity, parentPool);
                go.SetActive(false);
                cachedIds.Add(go.GetInstanceID(), key);
                cachedObjects[key].Push(go);
            }
            amount -= amountPerTick;
            
        }
    }

    public void SetParent(Transform parent)
    {
        parentPool = parent;
    }


    public GameObject Spawn(GameObject prefab, Vector3 position = default(Vector3),
        Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        var key = prefab.GetInstanceID();
        Stack<GameObject> stack;
        var stacked = cachedObjects.TryGetValue(key, out stack);

        if (stacked && stack.Count > 0)
        {
            Debug.Log("Ok");
            var transform = stack.Pop().transform;
            transform.SetParent(parent);
            transform.rotation = rotation;
            transform.gameObject.SetActive(true);
            if (parent) transform.position = position;
            else transform.localPosition = position;
            var poolable = transform.GetComponent<IPoolable>();
            if (poolable != null) poolable.OnSpawn();
            return transform.gameObject;
        }
        
        if (!stacked) cachedObjects.Add(key, new Stack<GameObject>());

        var createdPrefab = Populate(prefab, position, rotation, parent);
        cachedIds.Add(createdPrefab.GetInstanceID(), key);
        return createdPrefab;
    }


    public void Despawn(GameObject go)
    {
        go.SetActive(false);
        cachedObjects[cachedIds[go.GetInstanceID()]].Push(go);
        var poolable = go.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnDespawn();
        if (parentPool != null) go.transform.SetParent(parentPool);
    }



    GameObject Populate(GameObject prefab, Vector3 position = default(Vector3),
        Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        var go = Object.Instantiate(prefab, position, rotation, parent).transform;
        if (parent == null) go.position = position;
        else go.localPosition = position;
        return go.gameObject;
    }
}
