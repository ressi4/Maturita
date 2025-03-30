using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

        private int spawnDone = 0;
        public GameObject Platforma;

        public GameObject Hrac;

        public GameObject Enemy;

        public float MinXOffset = 15f;

        public float MaxXOffset = 27f;

        public float MinYOffset = 4f;

        public float MaxYOffset = 8f;

        public bool isFake = true;

        
void spawnNext(bool isFake = false)
{
    if (spawnDone>=2)
    {
        
        return;
    }


    
    float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.2f, 0)).y;
    float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.8f, 0)).y;
    float currentY = gameObject.transform.position.y;

    
    float bias = Mathf.InverseLerp(minY, maxY, currentY); 

    
    float direction = Random.value < bias ? -1f : 1f; 
    
    float yOffset = Random.Range(MinYOffset, MaxYOffset) * direction;
    float newY = currentY + yOffset;
    float newX = gameObject.transform.position.x + Random.Range(MinXOffset, MaxXOffset);

    
    if (newY > maxY)
    {
        newY = maxY - MinYOffset;
    }
    else if (newY < minY)
    {
        newY = minY + MinYOffset;
    }

    
    if(Random.value<0.2f){
        Instantiate(
        Enemy,
        new Vector2(newX + 4f, newY + 3f),
        Quaternion.identity
    );
    }

    
    GameObject GO = Instantiate(
        Platforma,
        new Vector2(newX, newY),
        Quaternion.identity
        
    );
        
    
    Platform p = GO.GetComponent<Platform>();
    p.isFake = isFake;
    p.Hrac = Hrac;
    p.Platforma = Platforma;

    spawnDone++;
}

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (spawnDone>=2)
    {
        
        return;
    }
        if(Hrac.transform.position.x>transform.position.x-35f){
            spawnNext(false);


        if(Random.value<0.1f & !isFake){
            
            spawnNext(true);

        }

        }
    }
}