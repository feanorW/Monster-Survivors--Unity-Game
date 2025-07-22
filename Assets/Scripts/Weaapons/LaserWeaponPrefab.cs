using System.Collections;
using UnityEngine;

public class LaserWeaponPrefab : MonoBehaviour
{
    public LaserWeapon weapon; // Reference to the LaserWeapon script

    private SpriteRenderer spriteRenderer; // Sprite renderer for the laser
    private CapsuleCollider2D capsuleCollider; // Capsule collider for the laser

    [SerializeField] private float laserGrowTime = .2f; // Time for the laser to grow to full size
    private float laserRange; // Range of the laser
    private float laserHeightSize;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        weapon = GameObject.Find("Laser Weapon").GetComponent<LaserWeapon>();
        laserRange = weapon.stats[weapon.weaponLevel].range; // Get the laser range from the weapon stats
        laserHeightSize = weapon.stats[weapon.weaponLevel].size; // Get the laser height size from the weapon stats

        AudioManager.Instance.PlaySound(AudioManager.Instance.laserWeapon); // Play the laser spawn sound effect

        StartCoroutine(IncreaseLaserLengthRoutine()); // Start the coroutine to increase the laser length
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage); // Deal damage to the enemy
            }
        }
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float elapsedTime = 0f;
        while (spriteRenderer.size.x < laserRange)
        {
            elapsedTime += Time.deltaTime;
            float linearT = elapsedTime / laserGrowTime; // Calculate linear interpolation factor

            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), laserHeightSize); // Grow the laser size

            capsuleCollider.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), laserHeightSize); // Grow the collider size
            capsuleCollider.offset = new Vector2((Mathf.Lerp(1f, laserRange, linearT) / 2f), capsuleCollider.offset.y); // Adjust the collider offset

            yield return null; // Wait for the next frame
        }
        spriteRenderer.size = new Vector2(laserRange, laserHeightSize); // Ensure the laser reaches its full size
        capsuleCollider.size = new Vector2(laserRange, laserHeightSize); // Ensure the collider reaches its full size
        capsuleCollider.offset = new Vector2((laserRange / 2f), capsuleCollider.offset.y); // Adjust the collider offset to match the full size

        yield return new WaitForSeconds(0.1f); // Wait a short time before starting the fade effect

        StartCoroutine(SlowFadeRoutine()); // Start the slow fade routine after the laser has fully grown
    }

    private IEnumerator SlowFadeRoutine()
    {
        float elapsedTime = 0f;
        float fadeDuration = .4f; // Duration for the fade effect
        float initialAlpha = spriteRenderer.color.a; // Get the initial alpha value of the sprite

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(initialAlpha, 0f, elapsedTime / fadeDuration); // Calculate the new alpha value
            Color newColor = spriteRenderer.color;
            newColor.a = alpha; // Set the new alpha value
            spriteRenderer.color = newColor; // Apply the new color to the sprite
            yield return null; // Wait for the next frame
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f); // Ensure the alpha is set to 0 at the end

        Destroy(gameObject); // Destroy the laser prefab after fading out
    }

    public void SetDirection(Vector3 direction)
    {
        transform.right = direction.normalized; // Set the direction of the laser
    }
}