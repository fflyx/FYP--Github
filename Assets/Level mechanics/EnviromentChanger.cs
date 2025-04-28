using UnityEngine;

public class EnvironmentChanger : MonoBehaviour
{
    public GameObject[] propsToToggle;
    public Light hallwayLight;

    public void OnNewLoop(int loopCount)
    {
        Debug.Log($"Loop {loopCount} entered!");

        if (loopCount == 1)
        {
            // Enable basic fog
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.gray;
            RenderSettings.fogDensity = 0.02f;
        }

        else if (loopCount == 2)
        {
            // Example: flicker lights
            hallwayLight.intensity = 0.2f;
            RenderSettings.fogDensity = 0.05f;
        }
        else if (loopCount == 3)
        {
            // Example: remove a prop
            if (propsToToggle.Length > 0)
            {
                propsToToggle[0].SetActive(false);
                RenderSettings.fogColor = new Color(0.05f, 0.05f, 0.1f); // Dark blue/gray
                RenderSettings.fogDensity = 0.1f;
            }
        }
        else if (loopCount == 5)
        {
            // Example: make it go DARK
            hallwayLight.enabled = false;
        }
    }
}
