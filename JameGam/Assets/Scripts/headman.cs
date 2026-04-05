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
        rb.linearVelocity = (playerScript.transform.position - transform.position).normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
         {
             playerScript.TakeDamage(10f); 
         }
    }
}
