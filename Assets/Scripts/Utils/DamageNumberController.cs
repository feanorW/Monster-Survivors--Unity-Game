using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public static DamageNumberController Instance;
    public ObjectPooler prefabPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateNumber(float value, Vector3 location)
    {
        //DamageNumber damageNumber = Instantiate(prefab, location, transform.rotation, transform);
        GameObject damageNumberObject = prefabPool.GetPooledObject();
        DamageNumber damageNumber = damageNumberObject.GetComponent<DamageNumber>();
        damageNumberObject.transform.position = location;
        damageNumberObject.transform.rotation = transform.rotation;
        damageNumberObject.SetActive(true); // Activate the damage number object
        damageNumber.SetDamage(value);
    }
}
