using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 direction;
    [SerializeField] private float moveSpeed;
    private ObjectPooler destroyEffectPool;
    [SerializeField] private float damage;
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private int experienceToGive;
    [SerializeField] private float pushTime;
    private float pushCounter;

    [SerializeField] private ObjectPooler dropHpPrefabPool;
    [SerializeField][Range(0f, 1f)] private float dropHpChance;

    [SerializeField] private ObjectPooler expPrefabPool;

    private void OnEnable()
    {
        health = maxHealth; // Reset health when the enemy is enabled
        pushCounter = 0f; // Reset push counter
        transform.rotation = Quaternion.identity; // Reset rotation
        moveSpeed = Mathf.Abs(moveSpeed); // Ensure move speed is positive
    }

    private void Start()
    {
        destroyEffectPool = GameObject.Find("EnemyDestroyEffectPool").GetComponent<ObjectPooler>();
        expPrefabPool = GameObject.Find("ExperienceObjectPool").GetComponent<ObjectPooler>();
        dropHpPrefabPool = GameObject.Find("HealthObjectPool").GetComponent<ObjectPooler>();
    }

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
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(damage); 
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        DamageNumberController.Instance.CreateNumber(damage, transform.position);
        pushCounter = pushTime;

        if (gameObject.activeInHierarchy)
            StartCoroutine(ChangeColor());

        if (health <= 0)
        {
            pushCounter = 0f; // Reset push counter when enemy is destroyed
            DestroyEffect();
            Drop();
            DropExperienceObject(experienceToGive);
            EnemySpawner.currentEnemyCount--;
            gameObject.SetActive(false); // Deactivate the enemy instead of destroying it
        }
        else
        {
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.enemyDamage); 
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
        //Instantiate(destroyEffect, transform.position, transform.rotation);
        GameObject destroyEffect = destroyEffectPool.GetPooledObject();
        destroyEffect.transform.position = transform.position;
        destroyEffect.transform.rotation = transform.rotation;
        destroyEffect.SetActive(true);

        spriteRenderer.color = Color.white;
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.enemyDie);
    }

    private void Drop()
    {
        if (Random.value <= dropHpChance && dropHpPrefabPool != null)
        {
            //Instantiate(dropHpPrefab, transform.position, Quaternion.identity);
            GameObject hpObject = dropHpPrefabPool.GetPooledObject();
            hpObject.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            hpObject.transform.rotation = Quaternion.identity;
            hpObject.SetActive(true);
        }
    }

    private void DropExperienceObject(int expAmount)
    {
        if (expPrefabPool == null || expAmount <= 0) return;

        for (int i = 0; i < expAmount; i++)
        {
            //Instantiate(expPrefab, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);

            GameObject expObject = expPrefabPool.GetPooledObject();
            expObject.transform.position = transform.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
            expObject.transform.rotation = Quaternion.identity;
            expObject.SetActive(true);
        }
    }
}
