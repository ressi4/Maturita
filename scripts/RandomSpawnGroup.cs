using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnableObject
{
    public GameObject obj;
}

public class RandomSpawnGroup : MonoBehaviour
{
    public SpawnableObject[] spawnables;
    public int minSpawnCount = 1;
    public int maxSpawnCount = 3;

void Start()
{
    GameObject player = GameObject.FindWithTag("Player");
    WeaponManager wm = player != null ? player.GetComponent<WeaponManager>() : null;

    bool playerHasWeapon = wm != null && wm.HasWeapon();

    List<GameObject> candidates = new List<GameObject>();

    foreach (var item in spawnables)
    {
        if (item.obj == null) continue;

        bool isWeapon = item.obj.CompareTag("Weapon");

        if (playerHasWeapon && isWeapon)
        {
            
            item.obj.SetActive(false);
            continue;
        }

        
        item.obj.SetActive(false);
        candidates.Add(item.obj);
    }

    ShuffleList(candidates);

    int spawnCount = Mathf.Clamp(Random.Range(minSpawnCount, maxSpawnCount + 1), 0, candidates.Count);

    for (int i = 0; i < spawnCount; i++)
    {
        candidates[i].SetActive(true);
    }
}




    void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
