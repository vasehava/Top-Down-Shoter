using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HitLogic : MonoBehaviour {

    public Text scoreText;
    public GameObject smokePrefab;
    public GameObject bloodPrefab;
    public ParticleSystem part;
    public ParticleCollisionEvent[] collisionEvents;
    public int damage = 20;
    public int countOfParticlesPool = 15;
    private int score = 0;
    private PlayerHealthController healthController = null;

    private List<DestroyOverTime> smokeList = new List<DestroyOverTime>();
    private List<DestroyOverTime> bloodList = new List<DestroyOverTime>();
    void Awake()
    {
        ManagerPool.Instance.AddPool(PoolType.SmokeParticles).PopulateWith(smokePrefab, 1, 1);
        ManagerPool.Instance.AddPool(PoolType.BloodParticles).PopulateWith(bloodPrefab, 1, 1);


    }
    void Start()
    {
        try
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new ParticleCollisionEvent[16];
            healthController = GetComponentInParent<PlayerHealthController>();

            for (int i = 0; i < countOfParticlesPool; i++)
            {
                smokeList.Add(ManagerPool.Instance.Spawn(PoolType.SmokeParticles, smokePrefab).GetComponent<DestroyOverTime>());
                smokeList[smokeList.Count - 1].OnSpawnEvent += ManagerPool.Instance.Spawn;
                smokeList[smokeList.Count - 1].OnDespawnEvent += ManagerPool.Instance.Despawn;

                bloodList.Add(ManagerPool.Instance.Spawn(PoolType.BloodParticles, bloodPrefab).GetComponent<DestroyOverTime>());
                bloodList[smokeList.Count - 1].OnSpawnEvent += ManagerPool.Instance.Spawn;
                bloodList[smokeList.Count - 1].OnDespawnEvent += ManagerPool.Instance.Despawn;

            }

        }
        catch { }
    }

    void OnParticleCollision(GameObject other)
    {
        int safeLength = part.GetSafeCollisionEventSize();
        if (collisionEvents.Length < safeLength)
            collisionEvents = new ParticleCollisionEvent[safeLength];

        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;
        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;

            if (rb)
            {
                
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }

            Quaternion rot = Quaternion.LookRotation(transform.position - pos);
            if (!other.CompareTag("Zombie"))
            {
                for (int j = 0; j < smokeList.Count; j++)
                    if (!smokeList[j].gameObject.activeSelf)
                    {
                        smokeList[j].Spawn(PoolType.SmokeParticles, smokePrefab, pos, rot);
                        break;
                    }
                //Instantiate(smokePrefab, pos, rot);
            }
            else
            {
                bool b;
                GameObject ob = null;
                for (int j = 0; j < smokeList.Count; j++)
                    if (!bloodList[j].gameObject.activeSelf)
                    {
                        bloodList[j].Spawn(PoolType.BloodParticles, bloodPrefab, pos, rot);
                        break;
                    }
                //Instantiate(bloodPrefab, pos, rot);
                other.GetComponent<IDamage>().ApplyDamage(damage, out b, out ob);

                if (b)
                {
                    if(healthController == null)
                        GetComponentInParent<PlayerHealthController>();

                    healthController.gameManager.AddScore(5, ref ob);
                }


            }
            i++;
        }

       
    }
}
