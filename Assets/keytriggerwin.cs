using UnityEngine;
using UnityEngine.SceneManagement;

public class WinOnPickup : MonoBehaviour
{
    public string winSceneName = "4 Victory"; // Name of your win scene

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player picked up the object. You win!");
            SceneManager.LoadScene(winSceneName);
        }
    }
}