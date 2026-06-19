using System.Collections;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float magnitude = 0.15f;

    private Vector3 restPos;
    private Coroutine routine;

    private void Awake()
    {
        restPos = transform.localPosition;
    }

    public void Shake()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = restPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = restPos;   
        routine = null;
    }
}