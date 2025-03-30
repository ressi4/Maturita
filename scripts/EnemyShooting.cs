using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public float shootingRange = 5f;
    public float shootCooldown = 2f;

    private GameObject hrac;
    private float timer = 0f;
    private bool playerInRange = false;

    void Start()
    {
        hrac = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (hrac == null) return;

        float distance = Vector2.Distance(transform.position, hrac.transform.position);

        if (distance <= shootingRange)
        {
            if (!playerInRange) 
            {
                Shoot();
                timer = 0;
                playerInRange = true;
            }

            timer += Time.deltaTime;
            if (timer > shootCooldown)
            {
                timer = 0;
                Shoot();
            }
        }
        else
        {
            playerInRange = false;
        }
    }

    void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
        
    }
}
