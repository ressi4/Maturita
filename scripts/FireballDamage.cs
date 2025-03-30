using UnityEngine;

public class FireballDamage : MonoBehaviour
{
    public int damageAmount = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerShield shield = collision.GetComponent<PlayerShield>();
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (shield != null && shield.IsShieldActive())
            {
                shield.BreakShield();
                Destroy(gameObject);
                
                return;
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                
            }

            Destroy(gameObject);
        }
    }
}
