using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 8f;

    [Header("Stamina / Sprint")]
    [SerializeField] private float maxStamina = 5f;   
    [SerializeField] private float drainRate = 1f;    
    [SerializeField] private float regenRate = 0.5f;  

    private Rigidbody2D rb;
    private Vector2 input;
    private float knockbackTimer;

    private float stamina;
    private bool exhausted;
    private bool isSprinting;

    // Para el HUD / barra de stamina
    public float StaminaNormalized => stamina / maxStamina;
    public bool IsExhausted => exhausted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stamina = maxStamina;
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        HandleStamina();

        if (knockbackTimer > 0f) return;

        Vector3 s = transform.localScale;
        if (input.x > 0.01f) s.x = Mathf.Abs(s.x);
        else if (input.x < -0.01f) s.x = -Mathf.Abs(s.x);
        transform.localScale = s;
    }

    private void HandleStamina()
    {
        bool wantsSprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool moving = input.sqrMagnitude > 0.01f;

        isSprinting = wantsSprint && moving && stamina > 0f && !exhausted;

        if (isSprinting)
        {
            stamina -= drainRate * Time.deltaTime;
            if (stamina <= 0f)
            {
                stamina = 0f;
                exhausted = true;   
            }
        }
        else
        {
            stamina += regenRate * Time.deltaTime;
            if (stamina >= maxStamina)
            {
                stamina = maxStamina;
                exhausted = false;  
            }
        }
    }

    private void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        float currentSpeed = isSprinting ? sprintSpeed : speed;
        rb.velocity = input * currentSpeed;
    }

    public void ApplyKnockback(Vector2 force, float duration = 0.25f)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
        knockbackTimer = duration;
    }
}