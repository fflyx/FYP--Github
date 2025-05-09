using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Button restartButton;
    public Button quitButton;

    void Start()
    {
        gameOverScreen.SetActive(false);  // Hide the game over screen at the start
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Call this method when the player dies (triggered from another script)
    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;  // Pause the game
    }

    void RestartGame()
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }

    void QuitGame()
    {
        Time.timeScale = 1f;  // Unpause the game
        Application.Quit();  // Quit the game
    }
}