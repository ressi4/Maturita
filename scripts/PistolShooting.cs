using UnityEngine;

public class PistolShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 3f;
    public AudioSource gunSound;

    private float nextFireTime = 0f;
    public bool isEquipped = true; 

    void Update()
    {
        if (!isEquipped) return;

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime)
        {
            Shoot();
            if (gunSound != null) gunSound.Play();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Chybí bulletPrefab nebo firePoint!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log(">>> Vystřelil jsem kulku");
    }
}
