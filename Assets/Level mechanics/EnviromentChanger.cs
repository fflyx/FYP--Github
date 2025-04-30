using UnityEngine;

public class EnvironmentChanger : MonoBehaviour
{
    public GameObject[] propsToToggle;
    public Light[] hallwayLights;
    
    public int loopCount = 0;

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
        else if (loopCount == 3)
        {
            
            if (propsToToggle.Length > 0)
            {
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
