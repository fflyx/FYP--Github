using UnityEngine;

public class EnvironmentChanger : MonoBehaviour
{
    public GameObject[] propsToToggle;
    public Light hallwayLight;
    public Light hallwayLight2;
    public Light hallwayLight3;
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
            
            hallwayLight.intensity = 0.2f;
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

            hallwayLight.enabled = false;
            hallwayLight2.enabled = false;
            hallwayLight3.enabled = false;
        }
    }
}
