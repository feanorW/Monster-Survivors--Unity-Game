using System.Drawing;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    [SerializeField] CollectibleType collectibleType; // Type of collectible (Weapon, Experience, Health)
    [SerializeField] Weapon objectWeapon;
    [SerializeField] int healthAmount;
    [SerializeField] private int experienceAmount;

    public void Collect()
    {


        if (collectibleType == CollectibleType.Weapon)
        {
            PlayerController.Instance.AddWeapon(objectWeapon);
            AudioManager.Instance.PlaySound(AudioManager.Instance.collectObject);
        }

        else if (collectibleType == CollectibleType.Health)
        {
            PlayerController.Instance.playerCurrentHealth += healthAmount;
            AudioManager.Instance.PlaySound(AudioManager.Instance.collectObject);
        }

        else if (collectibleType == CollectibleType.Experience)
        {
            PlayerController.Instance.GetExperience(experienceAmount);
        }

        if (collectibleType == CollectibleType.Weapon)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

enum CollectibleType
{
    Weapon,
    Health,
    Experience
}
