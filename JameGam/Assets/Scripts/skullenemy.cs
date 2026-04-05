using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class skullenemy : MonoBehaviour
{
    Player_Script playerScript;
    Rigidbody2D rb;

    public float speed = 5f;
    public float health = 20f;
    public float threshold = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 2f;
    public float attackRange = 1f;
    private float lastAttackTime = -Mathf.Infinity;
    public float distance;
    bool hasStartedCutScene = false;
    BoxCollider2D hitbox;
    BoxCollider2D hurtbox;
    Animator animator;
    float currentHealth;
    SpriteRenderer spriteRenderer;

    public Slider healthSlider;
    public bool isVulnerable = true;

    public bool startCutScene()
    {
        return playerScript.beginFadeIn("skull");
    }
    public void activateSkullHitbox()
    {
        isVulnerable = false;
        hitbox.enabled = true;

        Debug.Log("Skull hitbox activated. Player movement disabled.");
        spriteRenderer.color = Color.orange;
        healthSlider.fillRect.GetComponent<Image>().color = Color.gray;
    }

    public void deactivateSkullHitbox()
    {
        hitbox.enabled = false;
        isVulnerable = true;

        Debug.Log("Skull hitbox deactivated. Player movement enabled.");
        spriteRenderer.color = Color.white;
        healthSlider.fillRect.GetComponent<Image>().color = Color.red;
    }
    public void TakeDamage(float damage)
    {
        if(!isVulnerable)
        {
            return;
        }
        currentHealth -= damage;
        healthSlider.value = currentHealth / health;
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", 0.1f);
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
    void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }

    void Start()
    {
        playerScript = FindAnyObjectByType<Player_Script>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = transform.Find("SkullHitbox").GetComponent<BoxCollider2D>();
        hurtbox =GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = health;
        healthSlider.value = currentHealth / health;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, playerScript.transform.position);
        if (!hasStartedCutScene && distance <= threshold)
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

        if(distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Skull enemy is attacking the player.");
            animator.SetTrigger("skullattack");
            playerScript.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }
}
