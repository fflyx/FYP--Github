using UnityEngine;
using UnityEngine.SceneManagement;
public class playgame : MonoBehaviour
{
    private void Start()
    {
        
    }
    public void playGame1()
    {
        SceneManager.LoadScene("1 main menu");
    }

    public void quitGame()
    {
        Application.Quit();
    }
    private void Update()
    {
        
    }
}
