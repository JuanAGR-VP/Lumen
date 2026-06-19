using UnityEngine;
using UnityEngine.UI;

public class HeartsDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth player;
    [SerializeField] private Image[] hearts;   // tus corazones, en orden

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (player == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].enabled = i < player.CurrentLives;
        }
    }
}
