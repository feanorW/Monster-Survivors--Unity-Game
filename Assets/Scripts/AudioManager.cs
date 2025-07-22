using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource pause;
    public AudioSource resume;
    public AudioSource enemyDie;
    public AudioSource selectUpgrade;
    public AudioSource areaWeaponSpawn;
    public AudioSource areaWeaponDespawn;
    public AudioSource gameOver;
    public AudioSource enemyDamage;
    public AudioSource playerDamage;
    public AudioSource playerDie;
    public AudioSource levelUp;
    public AudioSource laserWeapon;
    public AudioSource spinningWeapon;
    public AudioSource collectObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void PlaySound(AudioSource sound)
    {
        sound.Stop();
        sound.Play();
    }

    public void PlayModifiedSound(AudioSource sound)
    {
        sound.pitch = Random.Range(0.7f, 1.3f); 
        sound.Stop();
        sound.Play();
    }
}
