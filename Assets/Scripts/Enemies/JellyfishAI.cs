using UnityEngine;


public class JellyfishAI : MonoBehaviour
{
    [Header("Deambular")]
    [SerializeField] private float wanderSpeed = 1f;
    [SerializeField] private float wanderRadius = 2f;
    [SerializeField] private float changeTargetTime = 3f;

    [Header("Electrocucion")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float shockRadius = 1.5f;
    [SerializeField] private float knockbackForce = 8f;

    [Header("Sonido")]
    [SerializeField] private AudioClip shockSound;

    private Transform player;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerAnimator playerAnimator;
    private Vector2 startPos;
    private Vector2 wanderTarget;
    private float timer;

    private void Awake()
    {
        startPos = transform.position;
        wanderTarget = startPos;

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerHealth = p.GetComponent<PlayerHealth>();
            playerMovement = p.GetComponent<PlayerMovement>();
            playerAnimator = p.GetComponent<PlayerAnimator>();
        }
        else
        {
            Debug.LogWarning("JellyfishAI: no encontre objeto con Tag 'Player'.");
        }
    }

    private void Update()
    {
        Wander();
        CheckShock();
    }

    private void Wander()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            wanderTarget = startPos + Random.insideUnitCircle * wanderRadius;
            timer = changeTargetTime;
        }

        transform.position = Vector2.MoveTowards(
            transform.position, wanderTarget, wanderSpeed * Time.deltaTime);
    }

    private void CheckShock()
    {
        if (player == null || playerHealth == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > shockRadius) return;

        bool hit = playerHealth.TakeDamage(damage);
        if (hit)
        {
            if (playerMovement != null)
            {
                Vector2 push = ((Vector2)player.position - (Vector2)transform.position).normalized;
                playerMovement.ApplyKnockback(push * knockbackForce);
            }
            if (playerAnimator != null) playerAnimator.Electrocute();
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(shockSound);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shockRadius);
        Gizmos.color = Color.cyan;
        Vector3 center = Application.isPlaying ? (Vector3)startPos : transform.position;
        Gizmos.DrawWireSphere(center, wanderRadius);
    }
}