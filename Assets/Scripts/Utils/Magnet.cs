using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float magnetRadius = 3f;
    [SerializeField] private LayerMask collectibleLayer;
    [SerializeField] private float speed = 2f;

    [SerializeField] private float boostedRadius;
    [SerializeField] private float boostDuration;

    private float boostTimer = 0f;
    private float currentRadius;

    private void FixedUpdate()
    {
        currentRadius = boostTimer > 0 ? boostedRadius : magnetRadius;
        float currentSpeed = boostTimer > 0 ? speed * 2 : speed;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentRadius, collectibleLayer);

        foreach (var hit in hits)
        {
            CollectibleObject collectible = hit.GetComponent<CollectibleObject>();

            if (collectible == null) continue;

            // Eğer boost modundaysa sadece HP ve XP çek
            if (boostTimer > 0)
            {
                if (collectible.Type != CollectibleType.Experience)
                    continue;
            }

            var anim = hit.GetComponent<CollectibleObjectAnimation>();
            if (anim != null) anim.enabled = false;

            Vector2 direction = (transform.position - hit.transform.position).normalized;
            hit.transform.position = Vector2.MoveTowards(hit.transform.position, transform.position, currentSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, hit.transform.position) < 0.1f)
            {
                if (anim != null) anim.enabled = true;
                collectible.Collect();
            }
        }

        if (boostTimer > 0)
        {
            boostTimer -= Time.fixedDeltaTime;
        }
    }

    public void ActivateBoost()
    {
        boostTimer = boostDuration;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }

}
