using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game_Manager : MonoBehaviour {

    public float startCountOfSlowZombies, startCountOfFastZombies, currentSlow, currentFast;
    public int startWaveTime = 40, waveTime;
    public int zombiesMaxCount = 30;
    public WaveRewardSystem rewardSystem;
    public GameObject gameplayCanvas;
    public GameObject menuCanvas;
    public GameObject bloodyScreen;

    public Transform player = null;
    public GameObject playerCamera;

    public Text timeToNextWave, waveNumber, score_text, zombiesCount_text;
    public Transform playerSpawnPoint;
    public List<Transform> zombieSpawnPoints = new List<Transform>();

    public GameObject playerPrefab, fastZombiePrefab, slowZombiePrefab;
    public SwipeGuns swipeGuns;

    public Image powerUpicon;
    public Slider powerUpslider;

    public GameObject topScoreUIPrefab;

    public GameObject medpackPrefab;
    public GameObject speedBoostPrefab;
    public int countOfBoost = 6;

    private List<Medpack> medpacks = new List<Medpack>();
    private List<SpeedBoost> speedBoosts = new List<SpeedBoost>();
    private List<ZombieController> fastZombeis = new List<ZombieController>();
    private List<ZombieController> tankZombies = new List<ZombieController>();

    private GameObject camera;
    private bool pause;
    //private List<GameObject> zombiesList = new List<GameObject>(200);
    private int zombieCountRightNow = 0;

    private int score = 0;
    private GameObject topScoreTable;
    // Use this for initialization

    void Awake()
    {
        ManagerPool.Instance.AddPool(PoolType.Medpack).PopulateWith(medpackPrefab, 1, 1);
        ManagerPool.Instance.AddPool(PoolType.SpeedBoost).PopulateWith(speedBoostPrefab, 1, 1);
        ManagerPool.Instance.AddPool(PoolType.FastZombie).PopulateWith(fastZombiePrefab, 1, 1);
        ManagerPool.Instance.AddPool(PoolType.TankZombie).PopulateWith(slowZombiePrefab, 1, 1);
    }


    void Start () {
        camera = GetComponentInChildren<Camera>().gameObject;
        waveNumber.text = "Wave: 1";
        timeToNextWave.text = "Next wave in " + waveTime.ToString() + "s";
        score_text.text = "Score: 0";
        currentFast = startCountOfFastZombies;
        currentSlow = startCountOfSlowZombies;
        waveTime = startWaveTime;
        topScoreTable = Instantiate(topScoreUIPrefab, menuCanvas.transform);

        for (int i = 0; i < countOfBoost; i++)
        {
            medpacks.Add(ManagerPool.Instance.Spawn(PoolType.Medpack, medpackPrefab).GetComponent<Medpack>());
            medpacks[medpacks.Count - 1].OnSpawnEvent += ManagerPool.Instance.Spawn;
            medpacks[medpacks.Count - 1].OnDespawnEvent += ManagerPool.Instance.Despawn;

            speedBoosts.Add(ManagerPool.Instance.Spawn(PoolType.SpeedBoost, speedBoostPrefab).GetComponent<SpeedBoost>());
            speedBoosts[speedBoosts.Count - 1].OnSpawnEvent += ManagerPool.Instance.Spawn;
            speedBoosts[speedBoosts.Count - 1].OnDespawnEvent += ManagerPool.Instance.Despawn;

        }

        for(int i = 0; i < zombiesMaxCount; i++)
        {
            fastZombeis.Add(ManagerPool.Instance.Spawn(PoolType.FastZombie, fastZombiePrefab, zombieSpawnPoints[i%zombieSpawnPoints.Count].position).GetComponent<ZombieController>());
            fastZombeis[fastZombeis.Count - 1].OnSpawnEvent += ManagerPool.Instance.Spawn;
            fastZombeis[fastZombeis.Count - 1].OnDespawnEvent += ManagerPool.Instance.Despawn;

            tankZombies.Add(ManagerPool.Instance.Spawn(PoolType.TankZombie, slowZombiePrefab, zombieSpawnPoints[i % zombieSpawnPoints.Count].position).GetComponent<ZombieController>());
            tankZombies[tankZombies.Count - 1].OnSpawnEvent += ManagerPool.Instance.Spawn;
            tankZombies[tankZombies.Count - 1].OnDespawnEvent += ManagerPool.Instance.Despawn;
        }
    }
	
	public void Play()
    {
        if(!pause)
        {
            gameplayCanvas.SetActive(true);
            menuCanvas.SetActive(false);
            player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).transform;
            for (int i = 0; i < zombiesMaxCount; i++)
            {
                fastZombeis[i].player_transform = player;
                tankZombies[i].player_transform = player;
            }
            playerCamera.SetActive(true);
            camera.SetActive(false);
            StartCoroutine(InstantiateZombies());
            StartCoroutine(ZombieGenerator());

        } else
        {
            pause = false;
            Time.timeScale = 1;
            menuCanvas.SetActive(false);
            gameplayCanvas.SetActive(true);
        }
    }

    public void Close()
    {
        Application.Quit();
    }

    public void Restart()
    {
        if (topScoreTable != null)
            Destroy(topScoreTable);
        bloodyScreen.SetActive(false);
        powerUpicon  .gameObject.SetActive(false);
        powerUpslider.gameObject.SetActive(false);
        player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).transform;
        playerCamera.SetActive(true);
        playerCamera.GetComponentInParent<FollowTarget>().target = player.transform;
        gameplayCanvas.SetActive(true);
        camera.SetActive(false);
        menuCanvas.SetActive(false);
        currentFast = startCountOfFastZombies;
        currentSlow = startCountOfSlowZombies;
        currentWaveNumber = 1;
        for (int i = 0; i < zombiesMaxCount; i++)
        {
            fastZombeis[i].player_transform = player;
            tankZombies[i].player_transform = player;
        }
        waveTime = startWaveTime;
        StartCoroutine(InstantiateZombies());
        StartCoroutine(ZombieGenerator()); waveNumber.text = "Wave: 1";
        timeToNextWave.text = "Next wave in " + waveTime.ToString() + "s";
        score_text.text = "Score: 0";
        score = 0;
        zombieCountRightNow = 0;

        for (int i = 0; i < countOfBoost; i++)
        {
            medpacks[i].Despawn();
            speedBoosts[i].Despawn();
        }
        swipeGuns.Init();
    }
    public void Pause()
    {
        pause = true;
        Time.timeScale = 0;
        menuCanvas.SetActive(true);
        gameplayCanvas.SetActive(false);
    }

    IEnumerator InstantiateZombies()
    {
        int f = (int)(currentFast);
        int s = (int)(currentSlow);
        for(int i = 0; i < zombieSpawnPoints.Count; i++)
        {
            yield return new WaitForEndOfFrame();
            if (f <= 0 && s <= 0)
                yield return 0;

            if(Vector3.Distance(zombieSpawnPoints[i].position, player.transform.position) > 30)
            {
                if(f > 0)
                {
                    for (int j = 0; j < fastZombeis.Count; j++)
                    {
                        if (!fastZombeis[j].gameObject.activeSelf)
                        {
                            print("f i: " + i);
                            //fastZombeis[j].Spawn(PoolType.FastZombie, fastZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation, player);
                            //zombiesList.Add(fastZombeis[j].gameObject);

                            //GameObject go = fastZombeis[j].gameObject;
                            fastZombeis[j].Spawn(PoolType.FastZombie, fastZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation);
                            //go.GetComponent<ZombieController>().player_transform = player;
                            //zombiesList.Add(fastZombeis[j].gameObject);
                            zombieCountRightNow++;
                            break;
                        }
                    }
                    //GameObject go = Instantiate(fastZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation);
                    //go.GetComponent<ZombieController>().player_transform = player;
                    //zombiesList.Add(go);
                    f--;
                }
                else if(s > 0)
                {
                    for (int j = 0; j < tankZombies.Count; j++)
                    {
                        if (!tankZombies[j].gameObject.activeInHierarchy)
                        {
                            print("s i: " + i);
                            //tankZombies[j].player_transform = player;
                            tankZombies[j].Spawn(PoolType.TankZombie, slowZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation);
                            //zombiesList.Add(tankZombies[j].gameObject);
                            zombieCountRightNow++;
                            break;
                        }
                    }
                    //GameObject go = Instantiate(slowZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation);
                    //go.GetComponent<ZombieController>().player_transform = player;
                    //zombiesList.Add(go);
                    //Instantiate(slowZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation).GetComponent<ZombieController>().player_transform = player;
                    s--;
                }
            }
            //print("zombies count: " + zombiesList.Count);
            if (i >= zombieSpawnPoints.Count - 1 && (f > 0 || s > 0))
                i = 0;
        }
        zombiesCount_text.text = "Zombies count: " + zombieCountRightNow.ToString();
    }
    int currentWaveNumber = 1;
    IEnumerator ZombieGenerator()
    {
        int waveCounter = 1;
        while(true)
        {
            for(int i = 1; i <= waveTime; i++)
            {
                yield return new WaitForSeconds(1);
                timeToNextWave.text = "Next wave in " + (waveTime - i).ToString() + "s";
            }
            //yield return new WaitForSeconds(40);
            this.currentFast *= 1.2f;
            this.currentSlow *= 1.2f;
            waveTime += 5;
            StartCoroutine(InstantiateZombies());
            waveNumber.text = "Wave: " + (currentWaveNumber = ++waveCounter).ToString();
        }
    }



    public void StopGenerateZombie()
    {
        StopAllCoroutines();
        if (topScoreTable != null)
        {
            Destroy(topScoreTable);
        }
        topScoreTable = Instantiate(topScoreUIPrefab, gameplayCanvas.transform);
        gameplayCanvas.GetComponentInChildren<TopScoreTable>().AddScore(score.ToString());
        topScoreTable.GetComponent<TopScoreTable>().OnDeath(score);


        for(int i = 0; i < medpacks.Count; i++)
        {
            if (medpacks[i].gameObject.activeSelf)
                medpacks[i].Despawn();
            if (speedBoosts[i].gameObject.activeSelf)
                speedBoosts[i].Despawn();
        }

        for (int i = 0; i < zombiesMaxCount; i++)
        {
            if (tankZombies[i].gameObject.activeSelf)
                tankZombies[i].Despawn();
            if (fastZombeis[i].gameObject.activeSelf)
                fastZombeis[i].Despawn();
        }
        zombieCountRightNow = 0;
        //if(topScoreTable != null)




    }

    public void AddScore(int count, ref GameObject zombie)
    {
        score += count;
        zombieCountRightNow--;
        Debug.Log(currentWaveNumber + " " + zombieCountRightNow);

        if(zombie != null)
            rewardSystem.CheckRewards(currentWaveNumber, zombieCountRightNow, zombie.transform.position);

        score_text.text = "Score: " + score;
        zombiesCount_text.text = "Zombies count: " + zombieCountRightNow.ToString();
    }

    public void ZombieBoostCreator(PoolType id, Vector3 pos, Quaternion rot)
    {
        switch (id)
        {
            case PoolType.Medpack:
                for(int i = 0; i < medpacks.Count; i++)
                {
                    if (!medpacks[i].gameObject.activeSelf)
                    {
                        medpacks[i].Spawn(PoolType.Medpack, medpackPrefab, pos, rot);
                        break;
                    }
                }
                break;

            case PoolType.SpeedBoost:
                for (int i = 0; i < medpacks.Count; i++)
                {
                    if (!speedBoosts[i].gameObject.activeSelf)
                    {
                        speedBoosts[i].Spawn(PoolType.SpeedBoost, speedBoostPrefab, pos, rot);
                        break;
                    }
                }
                break;

            default: break;
        }

    }
}
