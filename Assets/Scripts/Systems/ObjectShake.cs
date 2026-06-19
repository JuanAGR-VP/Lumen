using System.Collections;
using UnityEngine;

// ============================================================
//  ObjectShake (LUMEN).
//  Sacude la posicion local del objeto un momento. Pensado para el
//  efecto de electrocucion de los peces. Funciona porque el pez esta
//  quieto mientras lo sacuden, asi no pelea con la fisica.
//
//  USO: Assets/Scripts/Systems/ObjectShake.cs
//  - Ponlo en el pez (player, blindFish, NpcFish).
//  - Lo dispara su script al ser electrocutado: Shake(duracion).
// ============================================================
public class ObjectShake : MonoBehaviour
{
    [SerializeField] private float magnitude = 0.08f;

    private Vector3 restLocalPos;
    private Coroutine routine;
    private bool shaking;

    public void Shake(float duration)
    {
        // Captura la posicion de reposo solo si no estaba ya temblando
        if (!shaking) restLocalPos = transform.localPosition;
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShakeRoutine(duration));
    }

    private IEnumerator ShakeRoutine(float duration)
    {
        shaking = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = restLocalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = restLocalPos;
        shaking = false;
        routine = null;
    }
}
