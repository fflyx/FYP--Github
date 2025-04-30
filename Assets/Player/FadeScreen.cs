using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public Image fadeImage; 
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0); 
    }

    public void FadeOutAndIn(System.Action duringFadeAction)
    {
        StartCoroutine(FadeRoutine(duringFadeAction));
    }

    private IEnumerator FadeRoutine(System.Action duringFadeAction)
    {
        yield return StartCoroutine(Fade(0, 1)); 

        duringFadeAction?.Invoke(); 

        yield return StartCoroutine(Fade(1, 0)); 
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