using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        InvokeRepeating(nameof(Flicker), 0f, flickerSpeed);
    }

    void Flicker()
    {
        float chance = Random.value;
        if (chance < 0.1f)
            flickerLight.enabled = false;
        else
        {
            flickerLight.enabled = true;
            flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}