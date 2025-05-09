using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public static SceneTransitionManager singleton;

    private void Awake()
    {
        if (singleton && singleton != this)
            Destroy(singleton);

        singleton = this;
    }

    public void GoToScene()
    {
        SceneManager.LoadScene("2 gameplay");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        //Launch the new scene
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoToSceneAsync(int sceneIndex)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        
        //Launch the new scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
        while(timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
