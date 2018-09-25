using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HitLogic : MonoBehaviour {

    public Text scoreText;
    public GameObject smokePrefab;
    public GameObject bloodPrefab;
    public ParticleSystem part;
    public ParticleCollisionEvent[] collisionEvents;
    public int damage = 20;
    private int score = 0;
    private PlayerHealthController healthController = null;
    
    void Start()
    {
        try
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new ParticleCollisionEvent[16];
            healthController = GetComponentInParent<PlayerHealthController>();
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
                Instantiate(smokePrefab, pos, rot);
            else
            {
                bool b;
                GameObject ob = null;
                Instantiate(bloodPrefab, pos, rot);
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
