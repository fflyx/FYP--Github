using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnvironmentChanger : MonoBehaviour
{
    public AudioClip[] randomNoise;
    public AudioClip spawnSound;
    public AudioSource audioSource;
    public GameObject keyPrefab;
    public Transform[] keySpawnPoints;
    private GameObject spawnedKey;
    private bool keySpawned = false;
    public GameObject[] propsToToggle;
    public Light[] hallwayLights;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    private GameObject enemyInstance;
    private bool hasSpawnedLoop3 = false;
    private bool hasSpawnedLoop5 = false;
    public int loopCount = 0;
    private int loopkeySpawn = 0;

    void SpawnEnemy(bool temporary)
    {
        if (enemyInstance == null)
        {
            enemyInstance = Instantiate(enemyPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
        }
        else
        {
            enemyInstance.transform.position = enemySpawnPoint.position;
            enemyInstance.SetActive(true);
        }

        enemyInstance.GetComponent<EnemyBehaviour>().mode = EnemyBehaviour.EnemyMode.PassiveDisappear;
    }

    void Start()
    {
        loopkeySpawn = Random.Range(3, 6);
        audioSource = GetComponent<AudioSource>();
    }

    void playAudio()
    {
        if (randomNoise.Length == 0 || audioSource == null) return;

        int randomIndex = Random.Range(0, randomNoise.Length);
        AudioClip clip = randomNoise[randomIndex];

        audioSource.PlayOneShot(clip);
    }
    void SpawnKey()
    {
        int randomIndex = Random.Range(0, keySpawnPoints.Length);
        Transform spawnPoint = keySpawnPoints[randomIndex];
        spawnedKey = Instantiate(keyPrefab, spawnPoint.position, spawnPoint.rotation);
        keySpawned = true;
    }
    void ToggleProps()
    {
        if (propsToToggle.Length > 0)
        {
            
            propsToToggle[0].SetActive(false); 

            
            for (int i = 1; i < propsToToggle.Length; i++)
            {
                propsToToggle[i].SetActive(true); 
            }
        }
    }
    public void EnableProps()
    {
        foreach (GameObject prop in propsToToggle)
        {
            prop.SetActive(true);
        }
    }

    public void LoopNumber()
    {
        Debug.Log("Current Loop Count: " + loopCount);
        loopCount++;
        Debug.Log("Loop #" + loopCount);
        OnNewLoop(loopCount);
    }
    public void OnNewLoop(int loopCount)
    {

        Debug.Log($"Loop {loopCount} entered!");
      

        if (loopCount == 1)
        {
            

            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.gray;
            RenderSettings.fogDensity = 0.1f;
        }

        else if (loopCount == 2)
        {
            playAudio();
            foreach (Light light in hallwayLights)
            {
                light.intensity = 0.4f;
            }
            RenderSettings.fogDensity = 0.15f;
        }
        else if (loopCount == 3 && !hasSpawnedLoop3 && !keySpawned)
        {
            SpawnKey();
            playAudio();
            audioSource.PlayOneShot(spawnSound);
            SpawnEnemy(temporary: true); 
            hasSpawnedLoop3 = true;
            ToggleProps();
       
        }
        
        else if (loopCount == 5 && !hasSpawnedLoop5)
        {
            playAudio();
            audioSource.PlayOneShot(spawnSound);
            Debug.Log("Entering loop 5 logic...");
            RenderSettings.fogColor = new Color(0.05f, 0.05f, 0.1f);
            RenderSettings.fogDensity = 0.2f;

            EnableProps();
            SpawnEnemy(temporary: false);
            enemyInstance.GetComponent<EnemyBehaviour>().mode = EnemyBehaviour.EnemyMode.ChaseWhenUnseen;
            Debug.Log("Spawning enemy at loop 5...");
            hasSpawnedLoop5 = true;
            foreach (Light light in hallwayLights)
            {
                light.color = new Color(1f, 0f, 0f);
                light.intensity = 1f;
            }
        }

        else if (loopCount == 6)
        {
            playAudio();

            foreach (Light light in hallwayLights)
            {
                light.enabled = false;
            }
        }

        else if (loopCount == 7)
        {
            playAudio();
            SpawnEnemy(temporary: false);
            RenderSettings.fogDensity = 0.4f;
        }
    }
}
