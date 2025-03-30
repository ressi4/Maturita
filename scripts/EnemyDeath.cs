using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public GameObject deathParticles;
    private int health;
    private bool isDead = false;

    public GameObject hitEffect;

    private void Start()
    {
        
        if (CompareTag("Enemy"))
        {
            health = 2; // 2x pistol, 1x shotgun
        }
        else if (CompareTag("EnemyBig"))
        {
            health = 3; // 3x pistol, 2x shotgun
        }
        else
        {
            health = 1; 
        }
    }

   public void TakeDamage(int damage, WeaponType weaponUsed)
{
    if (isDead) return;

    health -= damage;

    Instantiate(hitEffect, transform.position, Quaternion.identity);

    if (health <= 0)
    {
        Die(weaponUsed);
    }
}



    public void Die(WeaponType weaponUsed)
{
    if (isDead) return;

    isDead = true;

    int scoreToAdd = 0;

    if (CompareTag("Enemy"))
    {
        scoreToAdd = weaponUsed == WeaponType.Pistol ? 100 : 50;
    }
    else if (CompareTag("EnemyBig"))
    {
        scoreToAdd = weaponUsed == WeaponType.Pistol ? 200 : 100;
    }

    
    GameObject player = GameObject.FindWithTag("Player");
    if (player != null)
    {
        script playerScript = player.GetComponent<script>();
        if (playerScript != null)
        {
            playerScript.AddScore(scoreToAdd);
        }
    }

    if (deathParticles != null)
    {
        GameObject effect = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
    }

    Destroy(gameObject);
}

}
