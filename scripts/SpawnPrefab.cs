using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public GameObject[] Prefabs;
    public bool spawnDone = false;
    public GameObject Hrac;
    public Vector2 spawnRange;
    public bool delayedSpawn = true; 

    void Update()
    {
        if (gameObject.transform.position.x - Hrac.transform.position.x < 20f && !spawnDone)
        {
            spawnDone = true;

            if (delayedSpawn)
            {
                Invoke("SpawnPlatform", 0.6f); 
            }
            else
            {
                SpawnPlatform(); 
            }
        }
    }

    void SpawnPlatform()
    {
        float end = gameObject.transform.position.x;
        if (gameObject.transform.childCount > 0)
        {
            end = gameObject.transform.GetChild(gameObject.transform.childCount - 1).transform.position.x;
        }

        Debug.Log(gameObject.name + " Spawning new platform " + end);
        GameObject Platforma = Prefabs[Random.Range(0, Prefabs.Length)];
        GameObject GO = Instantiate(
            Platforma,
            new Vector2(end + Random.Range(spawnRange.x, spawnRange.y), gameObject.transform.position.y),
            Quaternion.identity
        );

        SpawnPrefab SP = GO.AddComponent<SpawnPrefab>();
        SP.Hrac = Hrac;
        SP.Prefabs = Prefabs;
        SP.spawnRange = spawnRange;
        SP.delayedSpawn = false; 
    }
}
