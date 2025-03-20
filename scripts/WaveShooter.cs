using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveShooter : MonoBehaviour
{
    public GameObject wavePrefab; // Prefab vlny
    public float waveDuration = 2f; // Jak dlouho vlna zůstane na obrazovce
    public float waveInterval = 30f; // Interval mezi vlnami
    public float warningTime = 3f; // Čas mezi varováním a vlnou
    public Text warningText; // UI text pro varování
    public Transform waveSpawnPoint; // Místo, kde se vlna zobrazí

    void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waveInterval - warningTime);
            ShowWarning();
            yield return new WaitForSeconds(warningTime);
            SpawnWave();
        }
    }

    void ShowWarning()
    {
        if (warningText != null)
        {
            warningText.text = "⚠️ WAVE INCOMING! ⚠️";
            warningText.enabled = true;
        }
    }

    void SpawnWave()
    {
        if (warningText != null)
        {
            warningText.enabled = false; // Skryje varování
        }

        GameObject wave = Instantiate(wavePrefab, waveSpawnPoint.position, Quaternion.identity);
        wave.transform.localScale = new Vector3(Screen.width, 100, 1); // Nastaví šířku na celou obrazovku a výšku 100 px
        Destroy(wave, waveDuration); // Po určité době vlna zmizí
    }
}

