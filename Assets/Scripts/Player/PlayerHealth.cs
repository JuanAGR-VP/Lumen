using System.Collections;
using UnityEngine;

// PlayerHealth (LUMEN). Vidas + invulnerabilidad + feedback (shake,
// linterna, sonido de daño). USO: Assets/Scripts/Player/
public class PlayerHealth : MonoBehaviour
{
    [Header("Vidas")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invulnerabilityTime = 1.5f;

    [Header("Efectos")]
    [SerializeField] private LanternController lantern;
    [SerializeField] private CameraShake cameraShake;

    [Header("Sonido")]
    [SerializeField] private AudioClip hurtSound;

    private int currentLives;
    private bool isInvulnerable;

    public int CurrentLives => currentLives;
    public int MaxLives => maxLives;
    public bool IsInvulnerable => isInvulnerable;

    private void Awake()
    {
        currentLives = maxLives;
        if (lantern == null) lantern = GetComponentInChildren<LanternController>();
        if (cameraShake == null && Camera.main != null)
            cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    public bool TakeDamage(int amount)
    {
        if (isInvulnerable) return false;

        currentLives -= amount;
        Debug.Log("Vidas restantes: " + currentLives);

        if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(hurtSound);
        if (cameraShake != null) cameraShake.Shake();
        if (lantern != null) lantern.FlickerThenOff();

        if (currentLives <= 0)
        {
            Die();
            return true;
        }

        StartCoroutine(InvulnerabilityWindow());
        return true;
    }

    private IEnumerator InvulnerabilityWindow()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("Game Over");
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.ShowGameOver();
    }
}