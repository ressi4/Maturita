using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 2f;
    public WeaponType weaponType;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(float bulletSpeed)
    {
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (!collision.CompareTag("Player"))
    {
        EnemyDeath enemy = collision.GetComponent<EnemyDeath>();
        if (enemy != null)
        {
            enemy.TakeDamage(2, weaponType);
        }

        Destroy(gameObject);
    }
}

}
