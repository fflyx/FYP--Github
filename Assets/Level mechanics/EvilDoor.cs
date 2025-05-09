using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyExitTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the exit is the key
        if (other.CompareTag("Finish"))
        {
            // Log for debugging
            Debug.Log("Key touched the exit. You win!");

            // Load the win scene
            SceneManager.LoadScene("4 Victory");
        }
    }
}
