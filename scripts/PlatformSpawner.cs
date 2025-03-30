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
       
        float newX = lastSpawnPosition.x + xDistance;

        
        float randomY = Random.Range(minPlatformHeight, maxPlatformHeight);
        float newY = lastSpawnPosition.y + randomY + Random.Range(-2f, 3f); 

        
        newY = Mathf.Clamp(newY, minHeight, maxHeight);

        
        Vector3 spawnPosition = new Vector3(newX, newY, 0f);

        
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        
        lastSpawnPosition = new Vector3(newX, newY, 0f);
    }
}