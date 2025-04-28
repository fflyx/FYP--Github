using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class poop : MonoBehaviour
{
    public Image fadeImage; // Drag your black Image here
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0); // Start fully transparent
    }

    public void FadeOutAndIn(System.Action duringFadeAction)
    {
        StartCoroutine(FadeRoutine(duringFadeAction));
    }

    private IEnumerator FadeRoutine(System.Action duringFadeAction)
    {
        yield return StartCoroutine(Fade(0, 1)); // Fade to black

        duringFadeAction?.Invoke(); // Perform teleport while black

        yield return StartCoroutine(Fade(1, 0)); // Fade back to normal
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);

            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, endAlpha);
    }
}