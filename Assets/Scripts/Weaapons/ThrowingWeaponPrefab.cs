using UnityEngine;

public class ThrowingWeaponPrefab : MonoBehaviour
{
    public ThrowingWeapon weapon; // Reference to the ThrowingWeapon script

    private Vector3 direction;

    void Start()
    {
        weapon = GameObject.Find("Throwing Weapon").GetComponent<ThrowingWeapon>();
    }
    private void Update()
    {
        direction = PlayerController.Instance.playerMoveDirection;
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
            Destroy(gameObject); // Destroy the prefab after hitting an enemy
        }
    }

    void ThrowToDirectionOfPlayer()
    {
        // Calculate the direction to throw the weapon based on the player's movement direction
        Vector2 throwDirection = new Vector2(direction.x, direction.y).normalized;
        // Apply a force to the weapon in the throw direction
        //GetComponent<Rigidbody2D>().AddForce(throwDirection * weapon.stats[weapon.weaponLevel].throwForce, ForceMode2D.Impulse);
    }
}
