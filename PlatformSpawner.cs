using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab; 
    public Transform player; 
    public float xDistance = 6f; 
    public float minHeight = -4f; 
    public float maxHeight = 4f; 
    public float minPlatformHeight = 4f; 
    public float maxPlatformHeight = 8f; 

    private Vector3 lastSpawnPosition;

    void Start()
    {
        
        if (player != null)
        {
            lastSpawnPosition = player.position + new Vector3(2f, 0f, 0f);
        }
        

        
        SpawnPlatform();
    }

    void Update()
    {
        
        SpawnPlatform();
    }

    void SpawnPlatform()
    {
        // Určujeme pozici na ose X: poslední pozice + pevná vzdálenost
        float newX = lastSpawnPosition.x + xDistance;

        // Určujeme pozici na ose Y: posun mezi předchozími platformami s větší variabilitou
        float randomY = Random.Range(minPlatformHeight, maxPlatformHeight);
        float newY = lastSpawnPosition.y + randomY + Random.Range(-2f, 3f); // Přidání náhodného posunu pro větší variabilitu

        // Zajišťujeme, že Y pozice není mimo povolené limity
        newY = Mathf.Clamp(newY, minHeight, maxHeight);

        // Vytvoření pozice pro novou platformu
        Vector3 spawnPosition = new Vector3(newX, newY, 0f);

        // Vytvoření nové platformy
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        // Aktualizace poslední pozice pro novou platformu
        lastSpawnPosition = new Vector3(newX, newY, 0f);
    }
}