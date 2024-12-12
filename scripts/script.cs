using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    public float movSpeed = 5f;
    public float jumpForce = 15f;
    public Transform groundCheck; // Prázdný objekt pod postavou pro kontrolu země
    public float groundCheckRadius = 0.1f; // Poloměr raycastu pro kontrolu dotyku země
    public LayerMask groundLayer; // Vrstva označující zem

    public float normalGravity = 1f; // Normální gravitace
    public float fallGravity = 2.5f; // Zvýšená gravitace při pádu

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool jumpRequested;
    private bool isJumpBoostActive = false; // Sleduje, jestli je Jump Boost aktivní
    private bool isSpeedBoostActive = false; // Sleduje, jestli je Speed Boost aktivní

    bool facingRight = true; // Sledujeme směr, kterým se hráč dívá

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Pohyb hráče
        float speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
        rb.velocity = new Vector2(speedX, rb.velocity.y);

        // Skákání
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true; // Požadavek na skok
        }

        // Rychlý pád (držení šipky dolů)
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
        // Detekce, zda je hráč na zemi pomocí OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Pokud je požadavek na skok a hráč je na zemi, provedeme skok
        if (jumpRequested && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpRequested = false; // Resetujeme požadavek na skok
        }

        // Nastavení gravitace podle toho, zda stoupá nebo padá
        if (rb.velocity.y < 0) // Pokud hráč padá
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) // Pokud stoupá, ale nedrží Space
        {
            rb.gravityScale = fallGravity * 0.8f; // Lehce zvýšená gravitace při stoupání
        }
        else // Normální gravitace na zemi
        {
            rb.gravityScale = normalGravity;
        }
    }

    // Detekce při kontaktu s power-upy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpBoost"))
        {
            Debug.Log("Jump Boost sebrán!");
            StartCoroutine(ActivateJumpBoost());
            Destroy(collision.gameObject); // Zničíme objekt Jump Boost
        }
        else if (collision.CompareTag("SpeedBoost"))
        {
            Debug.Log("Speed Boost sebrán!");
            StartCoroutine(ActivateSpeedBoost(2f, 5f)); // Aktivujeme Speed Boost s násobitelem 2 a trváním 5 sekund
            Destroy(collision.gameObject); // Zničíme objekt Boots
        }
    }

    private void Flip()
    {
        facingRight = !facingRight; // Přepneme směr
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Otočíme hráče
        transform.localScale = scale;
    }

    // Logika Jump Boostu
    private IEnumerator ActivateJumpBoost()
    {
        if (isJumpBoostActive) yield break; // Pokud už je aktivní, nic nedělej

        isJumpBoostActive = true; // Nastavíme, že je Jump Boost aktivní
        float originalJumpForce = jumpForce; // Uložíme původní sílu skoku
        jumpForce *= 2f; // Zvýšíme sílu skoku (lze upravit faktor)

        yield return new WaitForSeconds(5f); // Boost trvá 5 sekund

        jumpForce = originalJumpForce; // Vrátíme původní sílu skoku
        isJumpBoostActive = false; // Resetujeme stav
    }

    // Logika Speed Boostu
    public IEnumerator ActivateSpeedBoost(float multiplier, float boostDuration)
    {
        if (isSpeedBoostActive) yield break; // Pokud už je aktivní, nic nedělej

        isSpeedBoostActive = true; // Nastavíme, že je Speed Boost aktivní
        float originalMovSpeed = movSpeed; // Uložíme původní rychlost
        movSpeed *= multiplier; // Zvýšíme rychlost pohybu

        yield return new WaitForSeconds(boostDuration); // Boost trvá určitou dobu

        movSpeed = originalMovSpeed; // Vrátíme původní rychlost
        isSpeedBoostActive = false; // Resetujeme stav
    }
}
