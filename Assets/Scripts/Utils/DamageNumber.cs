using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    private float floatSpeed; // Speed at which the damage number floats upwards

    void Start()
    {
        floatSpeed = Random.Range(0.5f, 1.5f); // Randomize the float speed for variety
        Destroy(gameObject, 1.0f); // Destroy the damage number after 1 second
    }

    private void Update()
    {
        // Move the damage number upwards over time
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void SetDamage(float damage)
    {
        damageText.text = damage.ToString("F0"); // Set the damage text, formatted as an integer
    }
}
