using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float gameTime;
    public bool gameActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        gameActive = true; // Set the game as active
    }

    private void Update()
    {
        if (gameActive)
        {
            gameTime += Time.deltaTime; // Increment game time
            UIController.Instance.UpdateTimerText(gameTime); // Update the timer text in the UI

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                PauseGame(); // Toggle pause when Escape is pressed
            }
        }
    }

    public void GameOver()
    {
        gameActive = false; // Set the game as inactive
        StartCoroutine(ShowGameOverScreen());
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(1f);
        UIController.Instance.gameOverPanel.SetActive(true);
        AudioManager.Instance.PlaySound(AudioManager.Instance.gameOver);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene"); // Reload the current scene
    }

    public void PauseGame()
    {
        if (UIController.Instance.pausePanel.activeSelf == false && UIController.Instance.gameOverPanel.activeSelf == false)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.pause); // Play pause sound
            Time.timeScale = 0f; // Pause the game
            UIController.Instance.pausePanel.SetActive(true);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.resume); // Play resume sound
            Time.timeScale = 1f; // Resume the game
            UIController.Instance.pausePanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Load the main menu scene
        Time.timeScale = 1f; // Ensure time scale is reset when returning to the main menu
    }
}