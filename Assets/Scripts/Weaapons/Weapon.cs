using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponLevel;
    public List<WeaponStats> stats;
    public Sprite weaponImage;

    public void LevelUp()
    {
        if (weaponLevel < stats.Count - 1)
        {
            weaponLevel++;
        }
    }
}

[System.Serializable]
public class WeaponStats
{
    public float cooldown;
    public float duration;
    public float damage;
    public float range;
    public float speed;
    public float size; //for spinning weapon and laser
    public int extraWeaponCount;
    public string description;
}


