using UnityEngine;

// VictoryGrieta (LUMEN). E para terminar el nivel (victoria). Muestra un
// prompt de "E" al acercarse. USO: Assets/Scripts/Environment/
[RequireComponent(typeof(Collider2D))]
public class VictoryGrieta : MonoBehaviour
{
    [Header("Interaccion")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject prompt;   // sprite de la tecla "E"

    private bool playerInRange;

    private void Awake()
    {
        if (prompt != null) prompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null) gm.ShowVictory();
            if (prompt != null) prompt.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (prompt != null) prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (prompt != null) prompt.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (prompt != null && prompt.activeSelf)
            prompt.transform.rotation = Quaternion.identity;
    }
}
