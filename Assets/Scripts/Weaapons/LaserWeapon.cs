using UnityEngine;

public class LaserWeapon : Weapon
{
    public ObjectPooler laserPrefabPool; // Prefab for the laser

    private float spawnCounter; // Counter for cooldown
    private int extraLaserCount;

    private void Update()
    {
        extraLaserCount = stats[weaponLevel].extraWeaponCount;
        spawnCounter -= Time.deltaTime;

        if (spawnCounter < 0)
        {
            spawnCounter = stats[weaponLevel].cooldown;

            Vector3 currentDirection = GetPrimaryDirection(PlayerController.Instance.lastDirection);

            // Ana lazer
            SpawnLaser(currentDirection);

            float angleStep = 360f / (extraLaserCount + 1);

            // Diğer lazerler
            for (int i = 1; i <= extraLaserCount; i++)
            {
                float angle = angleStep * i;
                Vector3 rotatedDirection = Quaternion.Euler(0, 0, angle) * currentDirection;
                SpawnLaser(rotatedDirection);
            }
        }
    }

    private void SpawnLaser(Vector3 dir)
    {
        GameObject laser = laserPrefabPool.GetPooledObject();
        laser.transform.position = transform.position;
        laser.transform.rotation = transform.rotation;
        laser.GetComponent<LaserWeaponPrefab>().SetDirection(dir);
        laser.SetActive(true);

        //GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity, transform);
    }

    private Vector3 GetPrimaryDirection(Vector3 rawDir)
    {
        if (Mathf.Abs(rawDir.x) > Mathf.Abs(rawDir.y))
            return new Vector3(Mathf.Sign(rawDir.x), 0, 0);
        else
            return new Vector3(0, Mathf.Sign(rawDir.y), 0);
    }

}
