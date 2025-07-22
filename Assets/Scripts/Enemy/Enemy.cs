using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 direction;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float damage;
    [SerializeField] private float health;
    [SerializeField] private int experienceToGive;
    [SerializeField] private float pushTime;
    private float pushCounter;

    [SerializeField] private GameObject dropHpPrefab;
    [SerializeField][Range(0f, 1f)] private float dropHpChance;

    [SerializeField] private GameObject expPrefab;

    void FixedUpdate()
    {
        if (PlayerController.Instance.gameObject.activeSelf)
        {
            if (PlayerController.Instance.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true; // Face right
            }
            else
            {
                spriteRenderer.flipX = false; // Face left
            }

            if (pushCounter > 0)
            {
                pushCounter -= Time.deltaTime; // Decrease the push counter
                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed;
                }
                if (pushCounter <= 0)
                {
                    moveSpeed = Mathf.Abs(moveSpeed); // Reset move speed after push time
                }
            }

            // Calculate the direction towards the player
            direction = (PlayerController.Instance.transform.position - transform.position).normalized;
            // Move the enemy towards the player
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop moving if the player is inactive
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(damage); // Deal damage to the player
        }
    }

    public void TakeDamage(float damage)
    {
        // Handle enemy taking damage here
        // For example, you can reduce health or destroy the enemy
        health -= damage;
        DamageNumberController.Instance.CreateNumber(damage, transform.position);
        pushCounter = pushTime; // Reset the push counter

        StartCoroutine(ChangeColor()); // Change color to indicate damage taken

        if (health <= 0)
        {
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.enemyDie);
            DestroyEffect();
            Destroy(gameObject);
            Drop();
            DropExperienceObject(experienceToGive);
        }
        else
        {
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.enemyDamage); // Play enemy damage sound
        }
    }

    private IEnumerator ChangeColor()
    {
        spriteRenderer.color = Color.red; // Change color to red
        yield return new WaitForSeconds(0.1f); // Wait for a short duration
        spriteRenderer.color = Color.white; // Change color back to white
    }

    private void DestroyEffect()
    {
        if (destroyEffect == null) return;

        Instantiate(destroyEffect, transform.position, transform.rotation);
    }

    private void Drop()
    {
        if (Random.value <= dropHpChance && dropHpPrefab !=null)
        {
            Instantiate(dropHpPrefab, transform.position, Quaternion.identity);
        }
    }

    private void DropExperienceObject(int expAmount)
    {
        if (expPrefab == null || expAmount <= 0) return;

        for (int i = 0; i < expAmount; i++)
        {
            Instantiate(expPrefab, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);
        }
    }
}
