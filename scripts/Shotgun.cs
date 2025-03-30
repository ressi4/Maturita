using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shotgun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float recoilAngle = 10f;
    public float recoilSpeed = 0.1f;
    public float knockbackForce = 5f;
    public AudioSource gunSound;

    public GameObject emptyEffectPrefab;
    public int ammo = 5;
    public int maxAmmo = 5;
    public Text ammoText;
    public bool isEquipped = false;

    private bool facingRight = true;
    private float nextFireTime = 0f;

    private Rigidbody2D playerRb;
    private script playerScript;
    public AudioClip emptyMagSound;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            playerScript = player.GetComponent<script>();
        }

        if (ammoText == null)
        {
            GameObject textObj = GameObject.Find("AmmoText");
            if (textObj != null)
                ammoText = textObj.GetComponent<Text>();
        }

    }

    private void Update()
    {
        
        if (!isEquipped || ammo <= 0) return;

        if (playerRb != null)
        {
            facingRight = playerRb.velocity.x >= 0;
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime)
        {
            Shoot();
            if (gunSound != null) gunSound.Play();
            nextFireTime = Time.time + 1f / fireRate;
            StartCoroutine(Recoil());
        }
    }

    private void Shoot()
{
    int bulletCount = 3;
    float spreadAngle = 3f; 

    GameObject[] bullets = new GameObject[bulletCount];

    for (int i = 0; i < bulletCount; i++)
    {
        float angleOffset = ((float)i - (bulletCount - 1) / 2f) * spreadAngle;
        Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, angleOffset);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
        bullets[i] = bullet;

        ShotgunBullet bulletScript = bullet.GetComponent<ShotgunBullet>();
        if (bulletScript != null)
        {
            bulletScript.weaponType = WeaponType.Shotgun;
            bulletScript.SetSpeed(30f);
        }
    }

    
    for (int i = 0; i < bullets.Length; i++)
    {
        for (int j = i + 1; j < bullets.Length; j++)
        {
            Physics2D.IgnoreCollision(
                bullets[i].GetComponent<Collider2D>(),
                bullets[j].GetComponent<Collider2D>()
            );
        }
    }

    ammo--;

    if (ammo > 0)
    {
        ApplyKnockback();
    }

    UpdateAmmoUI();

    if (ammo <= 0)
    {
        HandleWeaponDepleted();
    }
}


    public void UpdateAmmoUI()
    {
        if (ammoText == null) return;

        ammoText.gameObject.SetActive(isEquipped);
        ammoText.text = "AMMO: " + ammo;
    }

    private void HandleWeaponDepleted()
    {
        isEquipped = false;
        if (ammoText != null)
            ammoText.gameObject.SetActive(false);

        if (emptyMagSound != null && gunSound != null)
        {
            StartCoroutine(PlayEmptyClicks());
        }
    }

    private void ApplyKnockback()
    {
        if (playerRb != null && playerScript != null)
        {
            float knockbackDir = facingRight ? -1f : 1f;
            Vector2 force = new Vector2(knockbackDir * knockbackForce, 0);

            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(DisableMovementFor(0.2f));
        }
    }

    private IEnumerator DisableMovementFor(float duration)
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;
            yield return new WaitForSeconds(duration);
            playerScript.enabled = true;
        }
    }

    private IEnumerator Recoil()
    {
        transform.rotation = Quaternion.Euler(0, 0, recoilAngle);
        yield return new WaitForSeconds(recoilSpeed);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator PlayEmptyClicks()
    {
        for (int i = 0; i < 3; i++)
        {
            gunSound.PlayOneShot(emptyMagSound);
            yield return new WaitForSeconds(0.3f);
        }

        if (emptyEffectPrefab != null)
        {
            GameObject effect = Instantiate(emptyEffectPrefab, transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
            Destroy(effect, 1.5f);
        }

        Destroy(gameObject);
    }
    public void RefillAmmo(){
        ammo = maxAmmo;
    }
}
