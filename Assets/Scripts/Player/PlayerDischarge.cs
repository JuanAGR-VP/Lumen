using UnityEngine;


public class PlayerDischarge : MonoBehaviour
{
    [Header("Descarga")]
    [SerializeField] private float radius = 4f;
    [SerializeField] private float stunDuration = 4f;
    [SerializeField] private float cooldown = 20f;
    [SerializeField] private Transform dischargeOrigin;

    [Header("Animacion y sonido")]
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private AudioClip dischargeSound;

    private float cooldownTimer;

    public float CooldownNormalized => Mathf.Clamp01(1f - cooldownTimer / cooldown);
    public bool IsReady => cooldownTimer <= 0f;

    private void Awake()
    {
        if (playerAnimator == null) playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
            Discharge();
    }

    private void Discharge()
    {
        cooldownTimer = cooldown;

        Vector2 origin = dischargeOrigin != null
            ? dischargeOrigin.position
            : transform.position;

        MonoBehaviour[] all = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (MonoBehaviour mb in all)
        {
            if (mb is IStunnable stunnable)
            {
                if (Vector2.Distance(origin, mb.transform.position) <= radius)
                    stunnable.Stun(stunDuration);
            }
        }

        if (playerAnimator != null) playerAnimator.PlayDischarge();

        if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(dischargeSound);

        Debug.Log("Descarga! Aturde " + stunDuration + "s en radio " + radius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        Vector3 origin = dischargeOrigin != null
            ? dischargeOrigin.position
            : transform.position;

        Gizmos.DrawWireSphere(origin, radius);
    }
}