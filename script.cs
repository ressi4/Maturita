using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    public float movSpeed = 5f;
    public float jumpForce = 15f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public float normalGravity = 1f;
    public float fallGravity = 2.5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool jumpRequested;
    private bool isJumpBoostActive = false;
    public bool isSpeedBoostActive = false;

    private PlayerShield shield;
    bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        shield = GetComponent<PlayerShield>();
    }

    void Update()
    {
        float speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        rb.velocity = new Vector2(speedX, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }

        // Rychlý pád dolů při držení šipky dolů
        if (Input.GetKey(KeyCode.DownArrow))
        {
            float slowFallSpeed = movSpeed * 1.5f;
            rb.velocity = new Vector2(rb.velocity.x, -slowFallSpeed);
        }

        if (speedX > 0 && !facingRight)
        {
            Flip();
        }
        else if (speedX < 0 && facingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (jumpRequested && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpRequested = false;
        }

        // Nastavení gravitace podle pohybu hráče
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.gravityScale = fallGravity * 0.8f;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            if (shield != null && shield.IsShieldActive()) 
            {
                Destroy(collision.gameObject);
                Debug.Log("Štít zablokoval střelu!");
            }
            else
            {
                TakeDamage(1);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.CompareTag("JumpBoost"))
        {
            Debug.Log("Jump Boost sebrán!");
            StartCoroutine(ActivateJumpBoost());
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("SpeedBoost"))
        {
            Debug.Log("Speed Boost sebrán!");
            StartCoroutine(ActivateSpeedBoost(2f, 5f));
            Destroy(collision.gameObject);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Zvýšení skoku po omezenou dobu
    private IEnumerator ActivateJumpBoost()
    {
        if (isJumpBoostActive) yield break;

        isJumpBoostActive = true;
        float originalJumpForce = jumpForce;
        jumpForce *= 2f;

        yield return new WaitForSeconds(5f);

        jumpForce = originalJumpForce;
        isJumpBoostActive = false;
    }

    // Speed Boost – postupné zrychlení a zpomalení
    public IEnumerator ActivateSpeedBoost(float multiplier, float boostDuration)
    {
        if (isSpeedBoostActive) yield break;

        isSpeedBoostActive = true;
        float originalSpeed = movSpeed;
        float targetSpeed = originalSpeed * multiplier;

        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            movSpeed = Mathf.Lerp(originalSpeed, targetSpeed, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        movSpeed = targetSpeed;

        yield return new WaitForSeconds(boostDuration);

        elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            movSpeed = Mathf.Lerp(targetSpeed, originalSpeed, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        movSpeed = originalSpeed;

        isSpeedBoostActive = false;
    }

    private void TakeDamage(int damage)
    {
        Debug.Log("Hráč dostal zásah! HP - " + damage);
    }
}
