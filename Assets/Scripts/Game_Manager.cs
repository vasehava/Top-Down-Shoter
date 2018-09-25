using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game_Manager : MonoBehaviour {

    public float startCountOfSlowZombies, startCountOfFastZombies, currentSlow, currentFast;
    public int startWaveTime = 40, waveTime;

    public GameObject gameplayCanvas;
    public GameObject menuCanvas;
    public GameObject bloodyScreen;

    private Transform player = null;
    public GameObject playerCamera;

    public Text timeToNextWave, waveNumber, score_text, zombiesCount_text;
    public Transform playerSpawnPoint;
    public List<Transform> zombieSpawnPoints = new List<Transform>();

    public GameObject playerPrefab, fastZombiePrefab, slowZombiePrefab;
    public SwipeGuns swipeGuns;
    private GameObject camera;
    private bool pause;
    private List<GameObject> zombiesList = new List<GameObject>(200);

    private int score = 0;
    // Use this for initialization
    void Start () {
        camera = GetComponentInChildren<Camera>().gameObject;
        waveNumber.text = "Wave: 1";
        timeToNextWave.text = "Next wave in " + waveTime.ToString() + "s";
        score_text.text = "Score: 0";
        //print(zombiesList.Capacity);
        currentFast = startCountOfFastZombies;
        currentSlow = startCountOfSlowZombies;
        waveTime = startWaveTime;
	}
	
	public void Play()
    {
        if(!pause)
        {
            gameplayCanvas.SetActive(true);
            menuCanvas.SetActive(false);
            player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).transform;
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
        bloodyScreen.SetActive(false);
        player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).transform;
        playerCamera.SetActive(true);
        playerCamera.GetComponentInParent<FollowTarget>().target = player.transform;
        gameplayCanvas.SetActive(true);
        camera.SetActive(false);
        menuCanvas.SetActive(false);
        currentFast = startCountOfFastZombies;
        currentSlow = startCountOfSlowZombies;

        waveTime = startWaveTime;
        StartCoroutine(InstantiateZombies());
        StartCoroutine(ZombieGenerator()); waveNumber.text = "Wave: 1";
        timeToNextWave.text = "Next wave in " + waveTime.ToString() + "s";
        score_text.text = "Score: 0";
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
        //print(f + " " + s);
        for(int i = 0; i < zombieSpawnPoints.Count; i++)
        {
            yield return new WaitForEndOfFrame();
            if (f <= 0 && s <= 0)
                yield return 0;

            if(Vector3.Distance(zombieSpawnPoints[i].position, player.transform.position) > 30)
            {
                if(f > 0)
                {
                    GameObject go = Instantiate(fastZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation);
                    go.GetComponent<ZombieController>().player_transform = player;
                    zombiesList.Add(go);
                    f--;
                }
                else if(s > 0)
                {
                    GameObject go = Instantiate(slowZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation);
                    go.GetComponent<ZombieController>().player_transform = player;
                    zombiesList.Add(go);
                    //Instantiate(slowZombiePrefab, zombieSpawnPoints[i].position, zombieSpawnPoints[i].rotation).GetComponent<ZombieController>().player_transform = player;
                    s--;
                }
            }
            //print("zombies count: " + zombiesList.Count);
            if (i >= zombieSpawnPoints.Count - 1 && (f > 0 || s > 0))
                i = 0;
        }
        zombiesCount_text.text = "Zombies count: " + zombiesList.Count.ToString();
    }

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
            waveNumber.text = "Wave: " + (++waveCounter).ToString();
        }
    }

    IEnumerator DeleteZombies()
    {
        for(int i = 0; i < zombiesList.Count; i++)
        {
            yield return new WaitForEndOfFrame();
            Destroy(zombiesList[i]);
        }
        //print("count = " + zombiesList.Count);
        zombiesList.Clear();
    }

    public void StopGenerateZombie()
    {
        StopAllCoroutines();
        StartCoroutine(DeleteZombies());
    }
    public void AddScore(int count, ref GameObject zombie)
    {
        score += count;
        score_text.text = "Score: " + score;
        zombiesList.Remove(zombie);
        zombiesCount_text.text = "Zombies count: " + zombiesList.Count.ToString();
    }
}
