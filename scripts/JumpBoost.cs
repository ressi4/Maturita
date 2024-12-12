using UnityEngine;
using System.Collections;

public class JumpBoost : MonoBehaviour
{
    public float jumpMultiplier = 2f; // Faktor pro zvýšení skoku
    public float boostDuration = 5f; // Délka trvání power-upu

    private void OnTriggerEnter2D(Collider2D other) // Používáme Collider2D pro 2D
    {
        if (other.CompareTag("Player")) // Když hráč narazí na tento objekt
        {
            PlayerController player = other.GetComponent<PlayerController>(); 
            if (player != null)
            {
                Debug.Log("Jump boost aktivován!"); // Debugging: ověření aktivace
                StartCoroutine(BoostJump(player)); // Spuštění zvýšení skoku
                Destroy(gameObject); // Zničí JUMP objekt po použití
            }
        }
    }

    private IEnumerator BoostJump(PlayerController player)
    {
        float originalJumpForce = player.jumpForce; // Uložíme původní sílu skoku
        player.jumpForce += player.jumpForce * (jumpMultiplier - 1f); // Přidá menší boost
        yield return new WaitForSeconds(boostDuration); // Počkám na čas
        player.jumpForce = originalJumpForce; // Obnovíme původní sílu skoku
    }
}
