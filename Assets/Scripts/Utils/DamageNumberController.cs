using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public static DamageNumberController Instance;
    public DamageNumber prefab;

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
        DamageNumber damageNumber = Instantiate(prefab, location, transform.rotation, transform);
        damageNumber.SetDamage(value);
    }
}
