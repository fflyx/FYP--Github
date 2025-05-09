using System.Security.Cryptography;
using UnityEngine;

public class EnvironmentChanger : MonoBehaviour
{
    public GameObject[] propsToToggle;
    public Light[] hallwayLights;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    private GameObject enemyInstance;
    private bool hasSpawnedLoop3 = false;
    private bool hasSpawnedLoop5 = false;
    public int loopCount = 0;


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
 
    public void LoopNumber()
    {
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
            foreach (Light light in hallwayLights)
            {
                light.intensity = 0.4f;
            }
            RenderSettings.fogDensity = 0.15f;
        }
        else if (loopCount == 3 && !hasSpawnedLoop3)
        {

            if (propsToToggle.Length > 0)
            {
                SpawnEnemy(temporary: true);
                hasSpawnedLoop3 = true;
                propsToToggle[0].SetActive(false);

            }
        }
        else if (loopCount == 5 && !hasSpawnedLoop5)
        {
            RenderSettings.fogColor = new Color(0.05f, 0.05f, 0.1f);
            RenderSettings.fogDensity = 0.2f;

            SpawnEnemy(temporary: false);
            enemyInstance.GetComponent<EnemyBehaviour>().mode = EnemyBehaviour.EnemyMode.ChaseWhenUnseen;
            hasSpawnedLoop5 = true;
            foreach (Light light in hallwayLights)
            {
                light.color = new Color(1f, 0f, 0f);
                light.intensity = 1f;
            }
        }

        else if (loopCount == 6)
        {
            foreach (Light light in hallwayLights)
            {
                light.enabled = false;
            }
        }
    }
}
