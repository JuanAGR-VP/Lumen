using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Nivel a cargar al jugar")]
    [SerializeField] private string firstLevel = "1";

    [Header("Sonido")]
    [SerializeField] private AudioClip menuMusic;

    private void Start()
    {
        Time.timeScale = 1f;   // por si venimos de un juego pausado
        if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic(menuMusic);
    }

    // Boton "Jugar"
    public void PlayGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    // Boton "Salir"
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // detiene el Play en el editor
#else
        Application.Quit();
#endif
    }
}
