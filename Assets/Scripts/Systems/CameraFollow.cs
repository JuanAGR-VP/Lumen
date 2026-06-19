using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Seguimiento")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.15f; 

    [Header("Limites de la zona (opcional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private Camera cam;

    private Vector3 velocity;

    private void Awake()
    {
        if (target == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) target = p.transform;
        }
        if (cam == null) cam = GetComponentInChildren<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

   
        Vector3 goal = new Vector3(target.position.x, target.position.y, transform.position.z);

        if (useBounds && cam != null)
        {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;
            goal.x = Mathf.Clamp(goal.x, minBounds.x + halfW, maxBounds.x - halfW);
            goal.y = Mathf.Clamp(goal.y, minBounds.y + halfH, maxBounds.y - halfH);
        }

        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }

    public void SetBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
        useBounds = true;
    }

    public void SnapToTarget()
    {
        if (target == null) return;

        Vector3 goal = new Vector3(target.position.x, target.position.y, transform.position.z);
        if (useBounds && cam != null)
        {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;
            goal.x = Mathf.Clamp(goal.x, minBounds.x + halfW, maxBounds.x - halfW);
            goal.y = Mathf.Clamp(goal.y, minBounds.y + halfH, maxBounds.y - halfH);
        }
        transform.position = goal;
        velocity = Vector3.zero;
    }
}