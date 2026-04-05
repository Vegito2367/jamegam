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
    public float chaseAggression = 1.35f;
    public float preferredDistanceFromPlayer = 0.6f;
    public float separationRadius = 1.2f;
    public float separationStrength = 1.0f;
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
        if (playerScript == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

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

        if (distance > preferredDistanceFromPlayer)
        {
            Vector2 toPlayer = (playerScript.transform.position - transform.position).normalized;
            Vector2 separation = GetSeparationDirection();
            Vector2 moveDirection = (toPlayer * chaseAggression + separation * separationStrength).normalized;
            rb.linearVelocity = moveDirection * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if(distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Skull enemy is attacking the player.");
            animator.SetTrigger("skullattack");
            playerScript.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }

    Vector2 GetSeparationDirection()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        Vector2 separation = Vector2.zero;

        foreach (Collider2D nearby in nearbyColliders)
        {
            skullenemy otherSkullEnemy = nearby.GetComponentInParent<skullenemy>();
            if (otherSkullEnemy == null || otherSkullEnemy == this)
            {
                continue;
            }

            Vector2 offset = (Vector2)(transform.position - otherSkullEnemy.transform.position);
            float offsetDistance = offset.magnitude;
            if (offsetDistance > 0f)
            {
                separation += offset.normalized / offsetDistance;
            }
        }

        return separation;
    }
}
