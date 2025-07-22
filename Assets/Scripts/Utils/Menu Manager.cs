using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene"); // Reload the current scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}
