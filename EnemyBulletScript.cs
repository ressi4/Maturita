using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private GameObject hrac;
    private Rigidbody2D rb;
    public float force;
    public int damageAmount = 1; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hrac = GameObject.FindGameObjectWithTag("Player");

        if (hrac != null)
        {
            Vector3 direction = hrac.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
        else
        {
            Debug.LogWarning("Hráč nebyl nalezen ve scéně!");
        }
    }

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
            Debug.Log("Štít absorboval střelu a zmizel!");
            return; 
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Hráči bylo odebráno jedno srdíčko!");
        }

        Destroy(gameObject);
    }
}
}