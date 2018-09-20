using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour, IDamage {
    public int minDamage = 8;
    public int maxDamage = 16;

    public int maxHP = 100;

    public GameObject medpackPrefab = null;
    public int chance = 30;

    public Transform player_transform;
    public AudioClip[] audioClips;

    private AudioSource source;
    private NavMeshAgent zomb_Agent;
    private Animator animator;
    private int currentHP;
    private bool dead, attack;
    private new CapsuleCollider collider;

	// Use this for initialization
	void Start () {

        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        zomb_Agent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();

        zomb_Agent.SetDestination(player_transform.position);
        currentHP = maxHP;
        StartCoroutine("Sounds");
    }
	
	// Update is called once per frame
	void Update () {
        if (dead) return;

        if (player_transform != null && !attack)
            zomb_Agent.SetDestination(player_transform.position);

        if (zomb_Agent.remainingDistance <= 2)
        {
            zomb_Agent.isStopped = true;
            attack = true;
            animator.SetInteger("State", 1);
            StartCoroutine(Attack());
        }
        
    }

    void LateUpdate()
    {
        if (dead) return;

        animator.SetInteger("State", 0);
    }

    public void ApplyDamage(int damage, out bool killed, out GameObject obj)
    {
        obj = null;
        killed = false;
        if (dead) return;

        animator.SetInteger("State", 2);
        currentHP -= damage;

        if (currentHP <= 0)
        {
            killed = true;
            obj = gameObject;
            Die();
        }
    }

    public void Die()
    {
        dead = true;
        zomb_Agent.isStopped = true;
        collider.isTrigger = true;
        animator.SetInteger("State", 3);

        if (Random.Range(0, 100) <= chance)
            Instantiate(medpackPrefab, transform.position, transform.rotation);

        Destroy(gameObject, 5);
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);
        attack = false;
        zomb_Agent.isStopped = false;
    }
    private System.Collections.IEnumerator Sounds()
    {
        while(!dead)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 19.0f));
            source.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
        }
    }

    public void Heal(int h)
    {
        throw new System.NotImplementedException();
    }
    public void KillPlayer()
    {
        zomb_Agent.isStopped = true;
    }

    public void ApplyDamage(int damage, out bool killed)
    {
        throw new System.NotImplementedException();
    }

    public bool isDead { get { return dead; } }
}
