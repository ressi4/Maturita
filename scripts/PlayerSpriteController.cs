using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    public Sprite normalSprite; 
    public Sprite jumpSprite;   
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (rb.velocity.y > 0.1f || rb.velocity.y < -0.1f)
        {
            spriteRenderer.sprite = jumpSprite;
        }
        else
        {
            spriteRenderer.sprite = normalSprite;
        }
    }
}

