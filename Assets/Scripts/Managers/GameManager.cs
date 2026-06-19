using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject pausePanel; // NUEVO: Tu panel de pausa

    [Header("Sonido")]
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip levelMusic;

    private bool isPaused = false; // NUEVO: Rastrea si el juego está pausado

    private void Awake()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false); // Asegura que inicie apagado
        Time.timeScale = 1f;
    }

    private void Start()
    {
    
        if (SoundManager.Instance != null) SoundManager.Instance.PlayMusic(levelMusic);
    }


    private void Update()
    {

        bool canPause = !gameOverPanel.activeSelf && !victoryPanel.activeSelf;

        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(gameOverSound);
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(victorySound);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        if (SoundManager.Instance != null)
        SoundManager.Instance.StopSFX();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        if (SoundManager.Instance != null)
        SoundManager.Instance.StopSFX();
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        if (SoundManager.Instance != null)
        SoundManager.Instance.StopSFX();
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            GoToMenu();
    }
}