using UnityEngine;
using System.Collections.Generic;

public class InfiniteBackground : MonoBehaviour
{
    public GameObject backgroundPrefab; // Prefab pozadí
    public Transform cameraTransform; // Kamera
    public float speed = 2f; // Rychlost posunu
    private float backgroundWidth; // Šířka pozadí (stejná jako kamera)
    private List<GameObject> backgrounds = new List<GameObject>(); // Seznam aktivních pozadí

    void Start()
    {
        if (backgroundPrefab == null || cameraTransform == null)
        {
            Debug.LogError("❌ ERROR: Chybí `backgroundPrefab` nebo `cameraTransform` v Inspectoru!");
            return;
        }

        // Pozadí je stejně široké jako kamera
        backgroundWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        Debug.Log("📏 Background Width podle kamery: " + backgroundWidth);

        // Vytvoříme první dvě pozadí, aby bylo plynulé
        for (int i = 0; i < 2; i++)
        {
            float spawnX = i * backgroundWidth;
            GameObject bg = Instantiate(backgroundPrefab, new Vector3(spawnX, 0, 0), Quaternion.identity);
            backgrounds.Add(bg);
        }
    }

    void Update()
    {
        // Posouváme všechna pozadí
        foreach (GameObject bg in backgrounds)
        {
            bg.transform.position += Vector3.left * speed * Time.deltaTime;
        }

        // Zkontrolujeme, jestli kamera překročila poslední background
        GameObject lastBg = backgrounds[backgrounds.Count - 1]; // Nejvíc vpravo umístěný background
        if (cameraTransform.position.x >= lastBg.transform.position.x)
        {
            SpawnNewBackground(lastBg.transform.position.x + backgroundWidth);
        }
    }

    void SpawnNewBackground(float spawnX)
    {
        GameObject newBg = Instantiate(backgroundPrefab, new Vector3(spawnX, 0, 0), Quaternion.identity);
        backgrounds.Add(newBg);

        // Smazání starého backgroundu (volitelně)
        if (backgrounds.Count > 3) // Udržujeme maximálně 3 backgroundy
        {
            Destroy(backgrounds[0]);
            backgrounds.RemoveAt(0);
        }

        Debug.Log("🔄 Nový background spawnován na X: " + spawnX);
    }
}
