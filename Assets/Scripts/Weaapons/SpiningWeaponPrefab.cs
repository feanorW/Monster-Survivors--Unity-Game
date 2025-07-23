using UnityEngine;

public class SpiningWeaponPrefab : MonoBehaviour
{
    public SpiningWeapon weapon; // Reference to the AreaWeapon script
    private float timer; // Duration for which the prefab exists
    private float speed;
    private float damage;
    private float size;

    public Vector3 axis=new Vector3(0,0,1);

    void Awake()
    {
        gameObject.SetActive(false); // Deactivate the prefab initially
        weapon = GameObject.Find("Spining Weapon").GetComponent<SpiningWeapon>();
    }
    /*
    void Start()
    {
        speed = weapon.stats[weapon.weaponLevel].speed; // Get the spinning speed from the AreaWeapon script
        damage = weapon.stats[weapon.weaponLevel].damage; // Get the damage from the AreaWeapon script
        size = weapon.stats[weapon.weaponLevel].size; // Get the size from the AreaWeapon script
        transform.localScale = new Vector3(size, size, 1); // Set the size of the spinning weapon

    }*/

    void OnEnable()
    {
        timer = weapon.stats[weapon.weaponLevel].duration; // Get the duration from the AreaWeapon script
        AudioManager.Instance.PlaySound(AudioManager.Instance.spinningWeapon); // Play the sound effect
        speed = weapon.stats[weapon.weaponLevel].speed; // Get the spinning speed from the AreaWeapon script
        damage = weapon.stats[weapon.weaponLevel].damage; // Get the damage from the AreaWeapon script
        size = weapon.stats[weapon.weaponLevel].size; // Get the size from the AreaWeapon script
        transform.localScale = new Vector3(size, size, 1); // Set the size of the spinning weapon
    }

    private void Update()
    {
        SpinAroundPlayer();

        timer -= Time.deltaTime; // Decrease the timer
        if (timer <= 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false); // Deactivate the prefab instead of destroying it
        }
    }

    void SpinAroundPlayer()
    {
        gameObject.transform.RotateAround(weapon.transform.position, axis.normalized, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Deal damage to the enemy
            }
        }
    }
}