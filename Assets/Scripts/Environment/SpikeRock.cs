using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class SpikeRock : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 8f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerHealth hp = collision.gameObject.GetComponent<PlayerHealth>();
        if (hp == null) return;

        // Solo rebota si el daño realmente entro (no estaba invulnerable)
        bool hit = hp.TakeDamage(damage);
        if (hit)
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                Vector2 push = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized;
                pm.ApplyKnockback(push * knockbackForce);
            }
        }
    }
}
