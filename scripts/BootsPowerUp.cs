using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsPowerUp : MonoBehaviour
{
    public float speedMultiplier = 2f; 
    public float duration = 5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<script>().StartCoroutine(
                other.GetComponent<script>().ActivateSpeedBoost(speedMultiplier, duration)
            );
            Destroy(gameObject); // Zničí objekt Boots
        }
    }
}


