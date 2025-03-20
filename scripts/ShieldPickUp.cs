using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PlayerShield playerShield = collision.GetComponent<PlayerShield>();

            if (playerShield != null) 
            {
                playerShield.ActivateShield();
                Destroy(gameObject); 
            }
        }
    }
    
}
