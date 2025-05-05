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

        enemyInstance.GetComponent<EnemyBehaviour>().SetupTemporary(temporary);
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
                light.intensity = 0.2f;
            }
            RenderSettings.fogDensity = 0.25f;
        }
        else if (loopCount == 3 && !hasSpawnedLoop3)
        {
            
            if (propsToToggle.Length > 0)
            {
                SpawnEnemy(temporary: true);
                hasSpawnedLoop3 = true;
                propsToToggle[0].SetActive(false);
                RenderSettings.fogColor = new Color(0.05f, 0.05f, 0.1f); 
                RenderSettings.fogDensity = 0.5f;
            }
        }
        else if (loopCount == 5)
        {
            foreach (Light light in hallwayLights)
            {
                light.enabled = false;
            }
            
        }
    }
}
