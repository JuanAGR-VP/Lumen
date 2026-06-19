using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BlindFishAI : MonoBehaviour, IStunnable
{
    [Header("Persecucion")]
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private float chaseSpeed = 4.5f;

    [Header("Deambular (cuando no persigue)")]
    [SerializeField] private float idleSpeed = 1.2f;
    [SerializeField] private float idleWanderRadius = 2.5f;
    [SerializeField] private float idleChangeTime = 2.5f;

    [Header("Ataque")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float pauseAfterHit = 1.5f;

    [Header("Animacion / efectos")]
    [SerializeField] private Animator animator;
    [SerializeField] private ObjectShake objectShake;

    [Header("Sonido")]
    [SerializeField] private AudioClip lungeSound;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 startPos;
    private Vector2 wanderTarget;
    private float wanderTimer;
    private float pauseTimer;
    private float stunTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (objectShake == null) objectShake = GetComponent<ObjectShake>();
        startPos = transform.position;
        wanderTarget = startPos;

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogWarning("BlindFishAI: no encontre objeto con Tag 'Player'.");
    }

    private void FixedUpdate()
    {
        if (animator != null) animator.SetBool("IsStunned", stunTimer > 0f);

        if (stunTimer > 0f)
        {
            stunTimer -= Time.fixedDeltaTime;
            rb.velocity = Vector2.zero;
            return;
        }

        if (player == null) return;

        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.fixedDeltaTime;
            rb.velocity = Vector2.zero;
            return;
        }

        float dist = Vector2.Distance(rb.position, player.position);

        if (dist <= detectionRadius)
        {
            Vector2 dir = ((Vector2)player.position - rb.position).normalized;
            rb.velocity = dir * chaseSpeed;
            FaceMovement(dir.x);
        }
        else
        {
            wanderTimer -= Time.fixedDeltaTime;
            if (wanderTimer <= 0f)
            {
                wanderTarget = startPos + Random.insideUnitCircle * idleWanderRadius;
                wanderTimer = idleChangeTime;
            }

            Vector2 dir = wanderTarget - rb.position;
            if (dir.magnitude < 0.1f) rb.velocity = Vector2.zero;
            else { rb.velocity = dir.normalized * idleSpeed; FaceMovement(dir.x); }
        }
    }

    private void FaceMovement(float dirX)
    {
        if (Mathf.Abs(dirX) < 0.01f) return;
        Vector3 s = transform.localScale;
        s.x = dirX > 0 ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
        transform.localScale = s;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Vector2 push = ((Vector2)collision.transform.position - rb.position).normalized;
        PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
        if (pm != null) pm.ApplyKnockback(push * knockbackForce);

        PlayerHealth hp = collision.gameObject.GetComponent<PlayerHealth>();
        if (hp != null) hp.TakeDamage(damage);

        if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(lungeSound);

        pauseTimer = pauseAfterHit;
    }

    public void Stun(float duration)
    {
        stunTimer = duration;
        rb.velocity = Vector2.zero;
        if (objectShake != null) objectShake.Shake(duration);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}