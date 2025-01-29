using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsPowerUp : MonoBehaviour
{
    public float speedMultiplier = 1.2f; //boost
    public float duration = 5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            script playerScript = other.GetComponent<script>();
            if (playerScript != null && !playerScript.isSpeedBoostActive)
            {
                playerScript.StartCoroutine(playerScript.ActivateSpeedBoost(speedMultiplier, duration));
                Destroy(gameObject);
            }
        }
    }
}
