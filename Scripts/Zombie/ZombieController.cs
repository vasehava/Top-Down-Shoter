using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class ZombieController : PoolableObjectBehaviour, IDamage {

    public int minDamage = 8;
    public int maxDamage = 16;

    public int maxHP = 100;
    public int chanceOfDropBoost = 30;

    public Transform player_transform;
    public AudioClip[] audioClips;

    public AudioSource source;
    public NavMeshAgent zomb_Agent;
    public Animator animator;
    public new CapsuleCollider collider;

    public Image healthbarImage;
    private int currentHP;
    private bool dead, attack;
    private new Game_Manager gManager;
	// Use this for initialization
	void Start () {
        gManager = GameObject.Find("GameManager").GetComponent<Game_Manager>();
        source = GetComponent<AudioSource>();
        currentHP = maxHP;
        UpdateHealthBar();
        animator.SetInteger("State", 0);
        StartCoroutine("Sounds");
        collider.tag = "Zombie";
        player_transform = gManager.GetComponent<Game_Manager>().player;
        base.Despawn();

    }
	
	// Update is called once per frame
	void Update () {
        if (dead) return;

        if (player_transform != null && !attack)
        {
            zomb_Agent.SetDestination(player_transform.position);
        }

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

    void OnEnable()
    {
        currentHP = maxHP;
        UpdateHealthBar();
        animator.SetInteger("State", 0);
        StartCoroutine("Sounds");
        collider.tag = "Zombie";
        collider.isTrigger = false;
        dead = false;
        
    }
    public void Spawn(PoolType id, GameObject prefab, Vector3 pos, Quaternion rot, Transform target)
    {
        base.Spawn(id, prefab, pos, rot);
        player_transform = target;
        print(pos);
        print(player_transform.name);

    }


    public void ApplyDamage(int damage, out bool killed, out GameObject obj)
    {
        obj = null;
        killed = false;
        if (dead) return;
        animator.SetInteger("State", 2);
        currentHP -= damage;
        UpdateHealthBar();

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

        if (Random.Range(0, 100) <= chanceOfDropBoost)
        {
            switch (Random.Range(0, 2))
            {
                case 0: //Instantiate(medpackPrefab, transform.position, transform.rotation);
                    gManager.ZombieBoostCreator(PoolType.Medpack, transform.position, transform.rotation);
                    break;
                case 1: //Instantiate(speedBoostPrefab, transform.position, transform.rotation);
                    gManager.ZombieBoostCreator(PoolType.SpeedBoost, transform.position, transform.rotation);
                    break;
                default: break;
            }

        }


        StopAllCoroutines();
        Invoke("Despawn", 5);
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

    private void UpdateHealthBar()
    {
        if(currentHP <= 0)
        {
            healthbarImage.transform.parent.gameObject.SetActive(false);
            return;
        }

        if(!healthbarImage.transform.parent.gameObject.activeSelf)
            healthbarImage.transform.parent.gameObject.SetActive(true);

        healthbarImage.fillAmount = (float)currentHP / maxHP;
    }
}
