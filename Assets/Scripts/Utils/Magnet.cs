using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float magnetRadius = 3f;
    [SerializeField] private LayerMask collectibleLayer;
    [SerializeField] private float speed = 2f;

    private void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRadius, collectibleLayer);

        foreach (var hit in hits)
        {
            CollectibleObject collectible = hit.GetComponent<CollectibleObject>();
            if (collectible != null) 
            {
                // Disable animation while collecting
                CollectibleObjectAnimation animation = hit.GetComponent<CollectibleObjectAnimation>();
                animation.enabled = false;
                Vector2 direction = (transform.position - hit.transform.position).normalized;
                hit.transform.position = Vector2.MoveTowards(hit.transform.position, transform.position, speed * Time.fixedDeltaTime);

                if (Vector2.Distance(transform.position, hit.transform.position) < 0.1f)
                {
                    collectible.Collect(); // Collect the item
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }

}
