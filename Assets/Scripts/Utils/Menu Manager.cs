using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f); // Default volume if not set
            float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f); // Default volume if not set

            audioMixer.SetFloat("music", Mathf.Log10(musicVolume) * 20); // Convert linear value to logarithmic scale
            audioMixer.SetFloat("sfx", Mathf.Log10(sfxVolume) * 20); // Convert linear value to logarithmic scale
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene"); // Reload the current scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}
