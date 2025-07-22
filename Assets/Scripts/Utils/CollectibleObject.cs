using System.Drawing;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    [SerializeField] CollectibleType collectibleType; // Type of collectible (Weapon, Experience, Health)
    [SerializeField] Weapon objectWeapon;
    [SerializeField] int healthAmount;

    public void Collect()
    {

        AudioManager.Instance.PlaySound(AudioManager.Instance.collectObject);

        if (collectibleType == CollectibleType.Weapon)
        {
            PlayerController.Instance.weapons.Add(objectWeapon);
            UIController.Instance.AddWeaponToSlot(objectWeapon);
        }

        else if (collectibleType == CollectibleType.Health)
        {
            PlayerController.Instance.playerCurrentHealth += healthAmount;
        }

        Destroy(gameObject);
    }
}

enum CollectibleType
{
    Weapon,
    Health
}
