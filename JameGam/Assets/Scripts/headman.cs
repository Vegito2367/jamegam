using UnityEngine;
using UnityEngine.UI;

public class headman : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Player_Script playerScript;
    Rigidbody2D rb;

    public float speed = 5f;
    public float health = 20f;
    public float threshold = 2f;
    public float chaseAggression = 0.55f;
    public float preferredDistanceFromPlayer = 1.6f;
    public float separationRadius = 1.4f;
    public float separationStrength = 1.25f;

    float currentHealth;
    SpriteRenderer spriteRenderer;
    bool hasStartedCutScene = false;

    public Slider healthSlider;

    public bool startCutScene()
    {
        return playerScript.beginFadeIn("headman");
    }



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth/health;

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", 0.1f);
    }

    void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }

    void Start()
    {
        playerScript = FindAnyObjectByType<Player_Script>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = health;
        healthSlider.value = currentHealth/health;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerScript == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!hasStartedCutScene && Vector2.Distance(transform.position, playerScript.transform.position) <= threshold)
        {
            if (startCutScene())
            {
                hasStartedCutScene = true;
                rb.linearVelocity = Vector2.zero;
            }
        }
       
        if (playerScript.currentAnimation!="Idle")
        {
            rb.linearVelocity = Vector2.zero; 
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerScript.transform.position);
        if (distanceToPlayer <= preferredDistanceFromPlayer)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toPlayer = (playerScript.transform.position - transform.position).normalized;
        Vector2 separation = GetSeparationDirection();
        Vector2 moveDirection = (toPlayer * chaseAggression + separation * separationStrength).normalized;

        rb.linearVelocity = moveDirection * speed;
    }

    Vector2 GetSeparationDirection()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        Vector2 separation = Vector2.zero;

        foreach (Collider2D nearby in nearbyColliders)
        {
            headman otherHeadman = nearby.GetComponentInParent<headman>();
            if (otherHeadman == null || otherHeadman == this)
            {
                continue;
            }

            Vector2 offset = (Vector2)(transform.position - otherHeadman.transform.position);
            float distance = offset.magnitude;
            if (distance > 0f)
            {
                separation += offset.normalized / distance;
            }
        }

        return separation;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
         {
             playerScript.TakeDamage(10f); 
         }
    }
}
