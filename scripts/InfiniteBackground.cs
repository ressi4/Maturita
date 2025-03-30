using UnityEngine;
using System.Collections.Generic;

public class InfiniteBackground : MonoBehaviour
{
    public GameObject backgroundPrefab; 
    public Transform cameraTransform; 
    public float speed = 2f; 
    private float backgroundWidth; 
    private List<GameObject> backgrounds = new List<GameObject>(); 

    void Start()
    {
        if (backgroundPrefab == null || cameraTransform == null)
        {
            
            return;
        }

        
        backgroundWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
       

        
        for (int i = 0; i < 2; i++)
        {
            float spawnX = i * backgroundWidth;
            GameObject bg = Instantiate(backgroundPrefab, new Vector3(spawnX, 0, 0), Quaternion.identity);
            backgrounds.Add(bg);
        }
    }

    void Update()
    {
        
        foreach (GameObject bg in backgrounds)
        {
            bg.transform.position += Vector3.left * speed * Time.deltaTime;
        }

        
        GameObject lastBg = backgrounds[backgrounds.Count - 1]; 
        if (cameraTransform.position.x >= lastBg.transform.position.x)
        {
            SpawnNewBackground(lastBg.transform.position.x + backgroundWidth);
        }
    }

    void SpawnNewBackground(float spawnX)
    {
        GameObject newBg = Instantiate(backgroundPrefab, new Vector3(spawnX, 0, 0), Quaternion.identity);
        backgrounds.Add(newBg);

        
        if (backgrounds.Count > 3) 
        {
            Destroy(backgrounds[0]);
            backgrounds.RemoveAt(0);
        }

        
    }
    public void ResetBackground()
{
    
    foreach (GameObject bg in backgrounds)
    {
        Destroy(bg);
    }
    backgrounds.Clear();

    
    for (int i = 0; i < 2; i++)
    {
        float spawnX = i * backgroundWidth;
        GameObject bg = Instantiate(backgroundPrefab, new Vector3(spawnX, 0, 0), Quaternion.identity);
        backgrounds.Add(bg);
    }

    
}

}
