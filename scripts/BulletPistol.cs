using UnityEngine;

public class BulletPistol : MonoBehaviour
{
    public float speed = 50f;
    public float lifeTime = 2f;
    public WeaponType weaponType;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    public void SetSpeed(float bulletSpeed)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyDeath enemy = collision.GetComponent<EnemyDeath>();
        if (enemy != null)
        {
            enemy.TakeDamage(1, weaponType);
        }

        Destroy(gameObject);
    }
}
