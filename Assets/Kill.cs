using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoaderTrigger : MonoBehaviour
{
    public AudioClip triggerAudio;         
    public float delayBeforeSceneLoad = 5f; 
 

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (triggerAudio != null)
            {
                AudioSource.PlayClipAtPoint(triggerAudio, transform.position);
            }

            Invoke("LoadScene", delayBeforeSceneLoad);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("3 Game Over");
    }
}