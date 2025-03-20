using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HeartPickUp : MonoBehaviour
{
    public int healthIncrease = 1; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null) 
            {
                playerHealth.Heal(healthIncrease);
                Destroy(gameObject); 
            }
        }
    }
}
