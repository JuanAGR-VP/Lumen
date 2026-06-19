using UnityEngine;
using UnityEngine.UI;


public class AbilityIcon : MonoBehaviour
{
    [SerializeField] private PlayerDischarge discharge;
    [SerializeField] private Image icon;
    [SerializeField] private Image cooldownOverlay;

    private void Awake()
    {
        if (discharge == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) discharge = p.GetComponent<PlayerDischarge>();
        }
    }

    private void Update()
    {
        if (discharge == null) return;

        // Overlay = enfriamiento restante (lleno al usar, vacio cuando esta lista)
        if (cooldownOverlay != null)
            cooldownOverlay.fillAmount = 1f - discharge.CooldownNormalized;

        // Atenuar el icono cuando no esta lista
        if (icon != null)
            icon.color = discharge.IsReady ? Color.white : new Color(1f, 1f, 1f, 0.4f);
    }
}
