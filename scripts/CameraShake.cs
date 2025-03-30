using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    Vector3 originalPos;

    void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
