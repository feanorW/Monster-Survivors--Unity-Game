using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    private float floatSpeed; // Speed at which the damage number floats upwards

    void OnEnable()
    {
        floatSpeed = Random.Range(0.5f, 1.5f); // Randomize the float speed for variety
        StartCoroutine(Deactivate()); // Start the coroutine to deactivate the object after a delay
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

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second before deactivating
        gameObject.SetActive(false); // Deactivate the damage number object
    }
}
