using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TMP_Text healthText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Slider exprienceSlider;
    [SerializeField] private TMP_Text experienceText;
    public GameObject levelUpPanel;
    public LevelUpButton[] levelUpButtons;
    [SerializeField] private Slider backgroundMusicSlider;
    [SerializeField] private Slider sfxSlider;
    public AudioMixer audioMixer;
    [SerializeField] private TMP_Text musicValue;
    [SerializeField] private TMP_Text sfxValue;
    public WeaponSlot[] weaponsSlotsList;
    public Sprite healthIcon; // Icon for health upgrades
    public IncreaseHealthButton increaseMaxHealthButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume(); // Load saved music volume if it exists
        }
        else
        {
            SetMusicSlider();
            SetSFXSlider();
        }
    }

    public void UpdateHealthSlider()
    {
        playerHealthSlider.maxValue = PlayerController.Instance.playerMaxHealth;
        playerHealthSlider.value = PlayerController.Instance.playerCurrentHealth;
        healthText.text = $"{PlayerController.Instance.playerCurrentHealth} / {PlayerController.Instance.playerMaxHealth}";
    }
    public void UpdateTimerText(float gameTime)
    {
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timerText.text = $"{minutes}:{seconds:00}";
    }

    public void UpdateExperienceSlider()
    {
        exprienceSlider.maxValue = PlayerController.Instance.playerLevels[PlayerController.Instance.currentLevel -1];
        exprienceSlider.value = PlayerController.Instance.experience;
        experienceText.text = $"{exprienceSlider.value} / {exprienceSlider.maxValue}";
    }

    public void LevelUpPanelOpen()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void LevelUpPanelClose()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void SetMusicSlider()
    {
        float volume = backgroundMusicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20); // Convert linear value to logarithmic scale
        PlayerPrefs.SetFloat("musicVolume", volume); // Save the volume setting
        musicValue.text = $"{Mathf.RoundToInt(volume * 100)}"; // Update the text to show percentage
    }

    public void SetSFXSlider()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20); // Convert linear value to logarithmic scale
        PlayerPrefs.SetFloat("sfxVolume", volume); // Save the volume setting
        sfxValue.text = $"{Mathf.RoundToInt(volume * 100)}"; // Update the text to show percentage
    }

    private void LoadVolume()
    {
        backgroundMusicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicSlider();
        SetSFXSlider();
    }

    public void AddWeaponToSlot(Weapon weapon)
    {
        foreach (WeaponSlot slot in weaponsSlotsList)
        {
            if (!slot.isFull)
            {
                slot.weaponIcon.gameObject.SetActive(true); // Ensure the icon is visible
                slot.weaponIcon.sprite = weapon.weaponImage;
                slot.isFull = true;
                slot.weapon = weapon;
                return; // Exit after filling the first available slot
            }
        }
    }

    public void ChangeWeaponSlot(int newWeaponsSlotNumber, Weapon oldWeapon, Weapon newWeapon)
    {
        UpdateMainWeaponSlot(newWeapon);
        WeaponSlot slot = weaponsSlotsList[newWeaponsSlotNumber-1];
        slot.weaponIcon.sprite = oldWeapon.weaponImage;
        slot.weapon = oldWeapon;
    }

    public void UpdateMainWeaponSlot(Weapon weapon)
    {
        WeaponSlot mainSlot = weaponsSlotsList[0];
        mainSlot.weaponLevel.text = "Level " + (weapon.weaponLevel + 1);
        mainSlot.weaponIcon.sprite = weapon.weaponImage;
        mainSlot.isFull = true;
        mainSlot.weapon = weapon; // Update the weapon reference in the slot
    }
}
