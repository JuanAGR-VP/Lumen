using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;   // Light2D

[RequireComponent(typeof(Light2D))]
public class LanternController : MonoBehaviour
{
    [Header("Parpadeo")]
    [SerializeField] private float flickerDuration = 0.4f; 
    [SerializeField] private float flickerInterval = 0.06f; 

    [Header("Apagado")]
    [SerializeField] private float offDuration = 1.5f;      

    private Light2D lanternLight;
    private float baseIntensity;
    private Coroutine routine;

    private void Awake()
    {
        lanternLight = GetComponent<Light2D>();
        baseIntensity = lanternLight.intensity;
    }

    public void FlickerThenOff()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        // 1) Parpadeo: alterna encendido/apagado rapido
        float t = 0f;
        bool on = false;
        while (t < flickerDuration)
        {
            on = !on;
            lanternLight.intensity = on ? baseIntensity : 0f;
            yield return new WaitForSeconds(flickerInterval);
            t += flickerInterval;
        }

        // 2) Apagada un rato
        lanternLight.intensity = 0f;
        yield return new WaitForSeconds(offDuration);

        // 3) Se vuelve a encender
        lanternLight.intensity = baseIntensity;
        routine = null;
    }
}
