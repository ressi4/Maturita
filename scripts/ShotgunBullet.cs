using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f; // Po 2 sekundách zmizí

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed; // Pošle náboj ve směru, kam míří zbraň
        Destroy(gameObject, lifeTime); // Zničí náboj po určité době
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) // Aby se náboj nesrážel s hráčem
        {
            Destroy(gameObject);
        }
    }
}