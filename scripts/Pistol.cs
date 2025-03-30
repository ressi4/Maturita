using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pistol : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 4f;
    public float recoilAngle = 3f;
    public float recoilSpeed = 0.05f;
    public AudioSource gunSound;

    public GameObject emptyEffectPrefab;
    public int ammo = 12;
    public int maxAmmo = 12;
    public Text ammoText;
    public bool isEquipped = false;
    public AudioClip emptyMagSound;
    

    private float nextFireTime = 0f;
    private bool facingRight = true;

    private Rigidbody2D playerRb;
    private script playerScript;
    

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
            if (playerRb.velocity.x > 0) facingRight = true;
            else if (playerRb.velocity.x < 0) facingRight = false;
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
    if (firePoint == null) return;

    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    BulletPistol bulletScript = bullet.GetComponent<BulletPistol>();
    if (bulletScript != null)
    {
        bulletScript.weaponType = WeaponType.Pistol;
        bulletScript.SetSpeed(50f); 
    }

    ammo--;
    UpdateAmmoUI();

    if (ammo <= 0)
    {
        HandleWeaponDepleted();
    }
}

    public void UpdateAmmoUI()
    {
        
        if (ammoText == null) return;

        if (isEquipped)
        {
            
            ammoText.gameObject.SetActive(true);
            ammoText.text = "AMMO: " + ammo;
        }
        else
        {
            
            ammoText.gameObject.SetActive(false);
        }
    }

    private void HandleWeaponDepleted()
    {
        isEquipped = false;

        if (ammoText != null)
            ammoText.gameObject.SetActive(false);

        if (emptyMagSound != null && gunSound != null)
        {
            StartCoroutine(PlayEmptyClicksAndDestroy());
        }
    }

    private IEnumerator PlayEmptyClicksAndDestroy()
    {
        for (int i = 0; i < 3; i++)
        {
            gunSound.PlayOneShot(emptyMagSound);
            yield return new WaitForSeconds(0.3f);
        }

        if (emptyEffectPrefab != null)
        {
            Vector3 effectOffset = new Vector3(-1f, 0f, 0f);
            GameObject effect = Instantiate(emptyEffectPrefab, transform.position + effectOffset, Quaternion.identity);
            Destroy(effect, 1.5f);
        }

        Destroy(gameObject);
    }

    private IEnumerator Recoil()
    {
        transform.rotation = Quaternion.Euler(0, 0, recoilAngle);
        yield return new WaitForSeconds(recoilSpeed);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void RefillAmmo(){
        ammo = maxAmmo;
    }
}
