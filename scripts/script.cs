using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    public float movSpeed = 15f;
    public float jumpForce = 20f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public float normalGravity = 4f;
    public float fallGravity = 10f;

    public float jumpHoldTime = 0.2f; 
    private float jumpTimeCounter; 
    private bool isJumping;

    public Rigidbody2D rb;
    private bool isGrounded;
    private bool jumpRequested;
    private bool isJumpBoostActive = false;
    public bool isSpeedBoostActive = false;

    private PlayerShield shield;
    bool facingRight = true;

    public float dashDistance = 10f; 
    public float dashDuration = 0.3f;  
    public float dashCooldown = 5f;  
    private bool isDashing = false;
    private bool canDash = true;

    public GameObject cooldownEffectObject; // Cooldown objekt ve scéně (NE PREFAB!)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        shield = GetComponent<PlayerShield>();

        // **Ujistíme se, že cooldown efekt je na začátku vypnutý**
        if (cooldownEffectObject != null)
        {
            cooldownEffectObject.SetActive(false);
        }
    }

    void Update()
    {
        float speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        rb.velocity = new Vector2(speedX, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
            isJumping = true;
            jumpTimeCounter = jumpHoldTime;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

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

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        float startTime = Time.time;
        float dashSpeed = dashDistance / dashDuration; 

        float direction = facingRight ? 1f : -1f;

        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = new Vector2(direction * dashSpeed, rb.velocity.y);
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;

        // **Zapneme cooldown efekt a zajistíme, že je viditelný**
        if (cooldownEffectObject != null)
        {
            cooldownEffectObject.SetActive(true);

            // **Ujistíme se, že SpriteRenderer je viditelný**
            SpriteRenderer sr = cooldownEffectObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true;
            }
        }

        yield return new WaitForSeconds(dashCooldown); // Čekáme na cooldown

        // **Vypneme cooldown efekt**
        if (cooldownEffectObject != null)
        {
            cooldownEffectObject.SetActive(false);
        }

        canDash = true;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (jumpRequested && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequested = false;
        }

        if (rb.velocity.y < 0) 
        {
            rb.gravityScale = fallGravity;
            rb.velocity += Vector2.down * (fallGravity * Time.fixedDeltaTime * 2f);
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
        
        /* else if (collision.CompareTag("SpeedBoost"))
        {
            Debug.Log("Speed Boost sebrán!");
            StartCoroutine(ActivateSpeedBoost(2f, 5f));
            Destroy(collision.gameObject);
        } */
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    

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
