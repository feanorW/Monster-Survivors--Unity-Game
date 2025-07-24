using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed;
    public Vector3 playerMoveDirection;
    public Vector3 lastDirection;

    public float playerMaxHealth;
    [SerializeField] private float _playerCurrentHealth;
    public float playerCurrentHealth
    {
        get => _playerCurrentHealth;
        set
        {
            _playerCurrentHealth = Mathf.Clamp(value, 0f, playerMaxHealth);
            if (UIController.Instance != null)
                UIController.Instance.UpdateHealthSlider();
        }
    }
    public float healingTime;
    private float healingTimer;

    private bool isImmune;
    [SerializeField] private float immuneDuration;
    [SerializeField] private float immuneTimer;

    public int experience;
    public int currentLevel;
    public int maxLevel;
    public List<int> playerLevels;
    [SerializeField] private GameObject levelUpPrefab;

    public Weapon activeWeapon; //starting weapon

    public List<Weapon> weapons; // List of all weapons the player can use

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Volume volume;
    private Vignette vignette;

    void Awake()
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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        volume.profile.TryGet(out vignette); // Get the Vignette effect from the Volume profile

        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count-1] * 1.1f +15)); // Initialize player levels
        }
        playerCurrentHealth = playerMaxHealth;
        UIController.Instance.UpdateExperienceSlider(); // Initialize the experience slider in the UI
        UIController.Instance.AddWeaponToSlot(activeWeapon); // Initialize the weapon slot panel in the UI
    }

    void Update()
    {
        HealTimer();

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        playerMoveDirection = new Vector3(inputX, inputY).normalized;
        if(playerMoveDirection != Vector3.zero)
        {
            lastDirection = playerMoveDirection; // Update the last direction only when the player is moving
        }

        animator.SetFloat("moveX", inputX);
        animator.SetFloat("moveY", inputY);

        if (inputX != 0 || inputY != 0)
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }

        if (immuneTimer > 0)
        {
            immuneTimer -= Time.deltaTime; // Decrease the immune timer
        }
        else
        {
            isImmune = false; // Reset immune state when timer runs out
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(playerMoveDirection.x * moveSpeed, playerMoveDirection.y * moveSpeed);
    }

    public void TakeDamage(float damage)
    {
        if (!isImmune)
        {
            isImmune = true; // Set immune state to true
            immuneTimer = immuneDuration; // Reset the immune timer
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.playerDamage); // Play player hurt sound
            playerCurrentHealth -= damage;
            StartCoroutine(ChangeColorForDamage()); // Change color for damage effect

            if (playerCurrentHealth <= 0)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.playerDie); // Play player death sound
                GameManager.Instance.GameOver(); // Trigger game over when health reaches zero
                gameObject.SetActive(false); // Deactivate the player when health reaches zero
            }
        }
    }

    public void GetExperience(int experienceToGet)
    {
        experience += experienceToGet; // Add experience points
        UIController.Instance.UpdateExperienceSlider(); // Initialize the experience slider in the UI
        if (experience >= playerLevels[currentLevel -1]) // Check if the player has enough experience to level up
        {
            Instantiate(levelUpPrefab, transform.position+new Vector3(0,1), transform.rotation, transform); // Instantiate the level up effect
            LevelUp(); 
        }
    }

    private void LevelUp()
    {
        experience -= playerLevels[currentLevel - 1]; // Deduct the experience required for the next level
        currentLevel++; 
        UIController.Instance.UpdateExperienceSlider(); // Update the experience slider in the UI
        StartCoroutine(ShowLevvelUpScreen()); // Show the level up screen after a short delay
        AudioManager.Instance.PlaySound(AudioManager.Instance.levelUp); // Play level up sound
    }

    IEnumerator ShowLevvelUpScreen()
    {
        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < weapons.Count; i++)
        {
            UIController.Instance.levelUpButtons[i].ActivateButton(weapons[i]); // Activate the level up buttons for each weapon
        }
        UIController.Instance.increaseMaxHealthButton.ActivateButton(10); // Activate the increase max health button with a health increase of 10

        UIController.Instance.LevelUpPanelOpen(); // Open the level up panel
    }

    public void IncreaseMaxHealth(int healthIncrease)
    {
        playerMaxHealth += healthIncrease;
        playerCurrentHealth += 5;
    }

    private IEnumerator ChangeColorForDamage()
    {
        spriteRenderer.color = Color.gray;
        yield return StartCoroutine(VignetteFade(vignette.intensity.value, 0.4f, 0.1f));

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;
        yield return StartCoroutine(VignetteFade(vignette.intensity.value, 0.3f, 0.2f));
    }

    private IEnumerator VignetteFade(float from, float to, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float intensity = Mathf.Lerp(from, to, t);
            vignette.intensity.Override(intensity);
            yield return null;
        }

        vignette.intensity.Override(to); // tam değerle bitir
    }

    public void HealTimer()
    {
        healingTimer -= Time.deltaTime; // Decrease the healing timer
        if (healingTimer <= 0)
        {
            playerCurrentHealth += 1; // Heal the player by 1 health point
            healingTimer = healingTime; // Reset the healing timer
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        if (!weapons.Contains(weapon))
        {
            weapons.Add(weapon);
            UIController.Instance.AddWeaponToSlot(weapon);
            weapon.gameObject.SetActive(true);
        }
    }
}