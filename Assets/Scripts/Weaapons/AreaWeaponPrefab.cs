using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponPrefab : MonoBehaviour
{
    public AreaWeapon weapon; // Reference to the AreaWeapon script
    private Vector3 targetSize;
    private float timer; // Duration for which the prefab exists

    public List<Enemy> enemiesInRange;
    private float counter;

    private void Awake()
    {
        gameObject.SetActive(false); // Deactivate the prefab 
        weapon = GameObject.Find("Area Weapon").GetComponent<AreaWeapon>();
    }
    /*
    void Start()
    {
        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range;
        transform.localScale = Vector3.zero; // Set initial scale to zero
        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range; // Set target size based on weapon stats
        timer = weapon.stats[weapon.weaponLevel].duration; // Get the duration from the AreaWeapon script
    }*/

    private void OnEnable()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.areaWeaponSpawn); // Play the sound effect
        transform.localScale = Vector3.zero; // Reset scale to zero when enabled
        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range; // Set target size based on weapon stats
        timer = weapon.stats[weapon.weaponLevel].duration; // Get the duration from the AreaWeapon script
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime *5);

        timer -= Time.deltaTime; // Decrease the timer
        if (timer <= 0)
        {
            targetSize = Vector3.zero; // Set target size to zero for shrinking effect

            if (transform.localScale == Vector3.zero)
            {
                //Destroy(gameObject); // Destroy the object when it has shrunk to zero
                gameObject.SetActive(false); // Deactivate the prefab instead of destroying it
                enemiesInRange.Clear(); // Clear the list of enemies in range


                AudioManager.Instance.PlaySound(AudioManager.Instance.areaWeaponDespawn); // Play the destroy sound effect
            }
        }

        //periodic damage to enemies in range
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            counter = weapon.stats[weapon.weaponLevel].speed; // Reset the counter to the weapon's speed
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i] != null)
                {
                    enemiesInRange[i].TakeDamage(weapon.stats[weapon.weaponLevel].damage); // Deal damage to each enemy in range
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.GetComponent<Enemy>());
        }
    }
}
