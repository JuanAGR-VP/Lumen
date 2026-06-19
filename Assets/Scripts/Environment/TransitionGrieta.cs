using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TransitionGrieta : MonoBehaviour
{
    [Header("Destino")]
    [SerializeField] private Transform destination;
    [SerializeField] private Vector2 destMinBounds;
    [SerializeField] private Vector2 destMaxBounds;

    [Header("Interaccion")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject prompt;   // sprite de la tecla "E"

    [Header("Sonido")]
    [SerializeField] private AudioClip teleportSound;

    private bool playerInRange;
    private Transform player;

    private void Awake()
    {
        if (prompt != null) prompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
            Teleport();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform;
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

    private void Teleport()
    {
        if (destination == null || player == null) return;

        player.position = destination.position;

        CameraFollow cam = FindFirstObjectByType<CameraFollow>();
        if (cam != null)
        {
            cam.SetBounds(destMinBounds, destMaxBounds);
            cam.SnapToTarget();
        }

        if (SoundManager.Instance != null) SoundManager.Instance.PlaySFX(teleportSound);

        playerInRange = false;
        if (prompt != null) prompt.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (destination != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, destination.position);
            Gizmos.DrawWireSphere(destination.position, 0.3f);
        }
    }

    private void LateUpdate()
    {
        if (prompt != null && prompt.activeSelf)
            prompt.transform.rotation = Quaternion.identity;
    }
}