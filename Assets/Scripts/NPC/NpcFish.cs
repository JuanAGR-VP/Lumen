using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class NpcFish : MonoBehaviour, IStunnable
{
    [Header("Deambular")]
    [SerializeField] private float wanderSpeed = 1.5f;
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float changeTargetTime = 3f;

    [Header("Animacion / efectos")]
    [SerializeField] private Animator animator;
    [SerializeField] private ObjectShake objectShake;

    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 wanderTarget;
    private float timer;
    private float stunTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (objectShake == null) objectShake = GetComponent<ObjectShake>();
        startPos = transform.position;
        wanderTarget = startPos;
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

        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            wanderTarget = startPos + Random.insideUnitCircle * wanderRadius;
            timer = changeTargetTime;
        }

        Vector2 dir = wanderTarget - rb.position;
        if (dir.magnitude < 0.1f)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = dir.normalized * wanderSpeed;
            FaceMovement(dir.x);
        }
    }

    private void FaceMovement(float dirX)
    {
        if (Mathf.Abs(dirX) < 0.01f) return;
        Vector3 s = transform.localScale;
        s.x = dirX > 0 ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
        transform.localScale = s;
    }

    public void Stun(float duration)
    {
        stunTimer = duration;
        rb.velocity = Vector2.zero;
        if (objectShake != null) objectShake.Shake(duration);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = Application.isPlaying ? (Vector3)startPos : transform.position;
        Gizmos.DrawWireSphere(center, wanderRadius);
    }
}