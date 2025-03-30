using System.Collections;
using UnityEngine;

public class WaveShooter : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject warningUI;
    public Transform waveSpawnPoint;
    public Transform playerTransform;
    public float waveInterval = 5f;
    public float warningTime = 3f;
    public float fireballSpeed = 15f;
    public float fireballLifetime = 5f;

    private float targetY;
    private Coroutine waveRoutineRef;

    void Start()
    {
        StartWaveRoutine(); 
    }

    public void StartWaveRoutine()
    {
        if (waveRoutineRef == null)
        {
            waveRoutineRef = StartCoroutine(WaveRoutine());
            Debug.Log(" Vlny spuštěny.");
        }
    }

    public void StopWaves()
    {
        if (waveRoutineRef != null)
        {
            StopCoroutine(waveRoutineRef);
            waveRoutineRef = null;
            Debug.Log(" Vlny zastaveny.");
        }
    }

    IEnumerator WaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waveInterval - warningTime);

            if (playerTransform != null)
            {
                targetY = playerTransform.position.y;
            }

            ShowWarning();
            yield return new WaitForSeconds(warningTime);
            SpawnFireball();
        }
    }

    void ShowWarning()
    {
        if (warningUI != null)
        {
            StartCoroutine(BlinkWarning());
            Debug.Log(" Warning UI blikání začalo");
        }
    }

    IEnumerator BlinkWarning()
    {
        float blinkSpeed = 0.3f;
        float totalTime = warningTime;
        float elapsedTime = 0f;
        bool isVisible = false;

        while (elapsedTime < totalTime)
        {
            isVisible = !isVisible;

            Vector3 worldPos = new Vector3(playerTransform.position.x, targetY, playerTransform.position.z);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            Vector3 newPos = warningUI.transform.position;
            newPos.y = screenPos.y;
            warningUI.transform.position = newPos;

            warningUI.SetActive(isVisible);

            yield return new WaitForSeconds(blinkSpeed);
            elapsedTime += blinkSpeed;
        }

        warningUI.SetActive(false);
        Debug.Log(" Warning UI blikání hotovo");
    }

    void SpawnFireball()
    {
        if (playerTransform != null)
        {
            Vector3 spawnPos = waveSpawnPoint.position;
            spawnPos.y = targetY;

            GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.left * fireballSpeed;
            }

            Destroy(fireball, fireballLifetime);
            Debug.Log(" Fireball vystřelen na výšce: " + spawnPos.y);
        }
    }
}
