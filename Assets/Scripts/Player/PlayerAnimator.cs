using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private float moveThreshold = 0.1f;
    [SerializeField] private ObjectShake objectShake;

    private Animator animator;
    private Rigidbody2D rb;
    private float electrocutedTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (objectShake == null) objectShake = GetComponent<ObjectShake>();
    }

    private void Update()
    {
        bool moving = rb.velocity.sqrMagnitude > moveThreshold * moveThreshold;
        animator.SetBool("IsMoving", moving);

        if (electrocutedTimer > 0f) electrocutedTimer -= Time.deltaTime;
        animator.SetBool("IsElectrocuted", electrocutedTimer > 0f);
    }

    public void Electrocute(float duration = 0.5f)
    {
        electrocutedTimer = duration;
        if (objectShake != null) objectShake.Shake(duration);
    }

    public void PlayDischarge()
    {
        animator.SetTrigger("Discharge");
    }
}