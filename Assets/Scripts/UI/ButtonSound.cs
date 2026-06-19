using UnityEngine;
using UnityEngine.UI;

// ButtonSound (LUMEN). Reproduce un clic al presionar el boton.
// USO: Assets/Scripts/UI/  -> ponlo en cada Button y asigna el clip.
[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayClick);
    }

    private void PlayClick()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(clickSound);
    }
}
