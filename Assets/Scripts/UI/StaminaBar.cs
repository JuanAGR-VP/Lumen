using UnityEngine;
using UnityEngine.UI;

// ============================================================
//  Barra de stamina (LUMEN).
//  Lee la stamina del jugador y rellena una Image (tipo Filled).
//  Se pone roja cuando el pez esta agotado.
//
//  USO: Assets/Scripts/UI/StaminaBar.cs
//  - Ponlo en un objeto del Canvas.
//  - Asigna "fill" a la Image de relleno (Image Type = Filled).
//  - "player" se busca solo por el Tag "Player".
// ============================================================
public class StaminaBar : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Image fill;
    [SerializeField] private Color normalColor = Color.cyan;
    [SerializeField] private Color exhaustedColor = Color.red;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.GetComponent<PlayerMovement>();
        }
    }

    private void Update()
    {
        if (player == null || fill == null) return;
        fill.fillAmount = player.StaminaNormalized;
        fill.color = player.IsExhausted ? exhaustedColor : normalColor;
    }
}
