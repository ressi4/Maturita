using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject hitEffect;
    public AudioClip hitSound;
    public Text highScoreText;


    public GameObject cooldownEffectObject;
    [SerializeField] private AudioClip jumpSound;
    private AudioSource audioSource;

    private bool dashKillConfirmed = false;

    public Text scoreText;
    private int score = 0;
    private int bestScore = 0;

    private float distanceTracker = 0f;
    public float distanceStep = 1f;
    public int scorePerStep = 1;
    private Vector2 lastPosition;

    public Text scoreEndText;     
    public Text newHighScoreText; 
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        shield = GetComponent<PlayerShield>();
        audioSource = GetComponent<AudioSource>();

        if (cooldownEffectObject != null)
            cooldownEffectObject.SetActive(false);

        lastPosition = transform.position;

        if (highScoreText != null)
    {
        highScoreText.text = "HIGH SCORE: " + bestScore;
    }
    }

    void Update()
    {
  
if (highScoreText != null)
{
    highScoreText.text = "HIGH SCORE: " + bestScore;
}
        float speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        rb.velocity = new Vector2(speedX, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
            isJumping = true;
            jumpTimeCounter = jumpHoldTime;

            if (jumpSound != null && audioSource != null)
                audioSource.PlayOneShot(jumpSound);
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
            isJumping = false;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            StartCoroutine(Dash());

        if (Input.GetKey(KeyCode.DownArrow))
        {
            float slowFallSpeed = movSpeed * 1.5f;
            rb.velocity = new Vector2(rb.velocity.x, -slowFallSpeed);
        }

        if (speedX > 0 && !facingRight)
            Flip();
        else if (speedX < 0 && facingRight)
            Flip();

        float distanceThisFrame = transform.position.x - lastPosition.x;

        if (distanceThisFrame > 0f)
        {
            distanceTracker += distanceThisFrame;

            if (distanceTracker >= distanceStep)
            {
                int steps = Mathf.FloorToInt(distanceTracker / distanceStep);
                AddScore(steps * scorePerStep);
                distanceTracker -= steps * distanceStep;
            }
        }

        lastPosition = transform.position;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        dashKillConfirmed = false;

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

        if (!dashKillConfirmed)
        {
            if (cooldownEffectObject != null)
            {
                cooldownEffectObject.SetActive(true);
                SpriteRenderer sr = cooldownEffectObject.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.enabled = true;
            }

            yield return new WaitForSeconds(dashCooldown);

            if (cooldownEffectObject != null)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBig")))
        {
            EnemyDeath enemy = collision.gameObject.GetComponent<EnemyDeath>();
            if (enemy != null)
            {
                enemy.TakeDamage(999, WeaponType.None);
                dashKillConfirmed = true;

                score += 100;
                UpdateScoreUI();
            }
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score.ToString();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
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
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
    public void ResetScore()
{
    score = 0;
    UpdateScoreUI();
}
public void PlayerDied()
{
    bool isNewHighScore = false;

    if (score > bestScore)
    {
        bestScore = score;
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save();
        isNewHighScore = true;
    }

    if (highScoreText != null)
        highScoreText.text = "HIGH SCORE: " + bestScore;

    if (scoreEndText != null)
        scoreEndText.text = "YOUR SCORE: " + score;
        scoreEndText.gameObject.SetActive(true);

    if (newHighScoreText != null)
        newHighScoreText.gameObject.SetActive(isNewHighScore); 

    if (scoreText != null)
    scoreText.gameObject.SetActive(false);    
}


}
