using UnityEngine;

public class SpiningWeapon : Weapon
{
    public ObjectPooler weaponPrefab;

    private float spawnCounter;
    private int extraWeaponCount;

    void Update()
    {
        extraWeaponCount = stats[weaponLevel].extraWeaponCount;
        spawnCounter -= Time.deltaTime;

        if (spawnCounter < 0)
        {
            spawnCounter = stats[weaponLevel].cooldown;

            //Instantiate(weaponPrefab, transform.position + new Vector3(-2.7f, 0, 0), transform.rotation, transform);
            GameObject weapon = weaponPrefab.GetPooledObject();
            weapon.transform.position = transform.position + new Vector3(-2.7f, 0, 0);
            weapon.transform.rotation = transform.rotation;
            weapon.SetActive(true);

            for (int i = 0; i < extraWeaponCount; i++)
            {
                float angle = 360f / (extraWeaponCount + 1) * (i + 1);
                Vector3 rotatedPosition = Quaternion.Euler(0, 0, angle) * new Vector3(-2.7f, 0, 0);
                //Instantiate(weaponPrefab, transform.position + rotatedPosition, transform.rotation, transform);
                GameObject weaponE = weaponPrefab.GetPooledObject();
                weaponE.transform.position = transform.position + rotatedPosition;
                weaponE.transform.rotation = transform.rotation;
                weaponE.SetActive(true);
            }
        }
    }
}
