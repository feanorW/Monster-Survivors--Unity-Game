using UnityEngine;

public class AreaWeapon : Weapon
{
    [SerializeField] private ObjectPooler prefabPool;
    private float spawnCounter;

    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter < 0)
        {
            spawnCounter = stats[weaponLevel].cooldown;
            //Instantiate(prefab, transform.position, transform.rotation, transform);
            GameObject areaWeapon = prefabPool.GetPooledObject();
            areaWeapon.transform.position = transform.position;
            areaWeapon.SetActive(true);

        }
    }
}
