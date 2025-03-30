using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    public float speed = 15f;             
    public float lifeTime = 3f;          

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
