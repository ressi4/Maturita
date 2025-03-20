using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab náboje
    public Transform firePoint; // Místo, odkud se střílí
    public float fireRate = 1f; // Interval mezi výstřely
    public float recoilAngle = 10f; // Úhel zpětného rázu
    public float recoilSpeed = 0.1f; // Rychlost zpětného rázu

    private float nextFireTime = 0f;
    public AudioSource gunSound;
    public Rigidbody2D playerRb;
    public float knockbackForce = 5f;

    private script playerScript; // Odkaz na skript hráče
    private bool facingRight = true; // Směr hráče

    private void Start()
    {
        playerScript = playerRb.GetComponent<script>(); // Získá skript hráče
    }

    private void Update()
    {
        // Zkontroluje směr hráče
        if (playerRb.velocity.x > 0)
        {
            facingRight = true;
        }
        else if (playerRb.velocity.x < 0)
        {
            facingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime) // Střelba klávesou E
        {
            Shoot();
            if (gunSound != null) gunSound.Play();
            nextFireTime = Time.time + 1f / fireRate; // Nastaví cooldown
            StartCoroutine(Recoil()); // Spustí animaci zpětného rázu
        }
    }

    private void Shoot()
    {
        // Vytvoří střelu
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Dává náboji rychlost
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.right * 10f; // Střela letí dopředu
        }

        ApplyKnockback(); // Knockback po výstřelu
    }

    private void ApplyKnockback()
    {
        if (playerRb != null && playerScript != null)
        {
            float knockbackDirection = facingRight ? -1f : 1f; // Knockback směrem dozadu
            Vector2 knockbackForceVector = new Vector2(knockbackDirection * knockbackForce, 0);

            playerRb.velocity = Vector2.zero; // Reset rychlosti, aby knockback fungoval
            playerRb.AddForce(knockbackForceVector, ForceMode2D.Impulse);

            StartCoroutine(DisableMovementFor(0.2f)); // Na chvíli vypne ovládání hráče
        }
        else
        {
            Debug.LogWarning("Hráč nemá přiřazený Rigidbody2D nebo skript hráče!");
        }
    }

    private IEnumerator DisableMovementFor(float duration)
    {
        if (playerScript != null)
        {
            playerScript.enabled = false; // Vypne ovládání hráče
            yield return new WaitForSeconds(duration);
            playerScript.enabled = true; // Znovu zapne ovládání hráče
        }
    }

    private IEnumerator Recoil()
    {
        transform.rotation = Quaternion.Euler(0, 0, recoilAngle);
        yield return new WaitForSeconds(recoilSpeed);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
