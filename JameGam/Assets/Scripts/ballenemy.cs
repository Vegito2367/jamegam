using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ballenemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform ball;
    Transform lady;

    Rigidbody2D rb;
    Player_Script player;
    CircleCollider2D hitbox;

    public float maxDistanceBetweenPlayer = 5f;
    public float speed = 2f;
    public float separationRadius = 1.2f;
    public float separationStrength = 1.5f;
    public float randomSteerStrength = 0.35f;
    public float randomSteerUpdateInterval = 0.4f;
    public float health=10f;
    float currentHealth;
    public Slider healthSlider;

    bool isBallMode = true;
    Vector2 randomSteerDirection = Vector2.zero;
    float nextRandomSteerUpdateTime = 0f;

    void Start()
    {
        ball = transform.Find("Ball");
        lady = transform.Find("Lady");
        player = FindAnyObjectByType<Player_Script>();    
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CircleCollider2D>();
        currentHealth = health;
        healthSlider.value = currentHealth / health;
        healthSlider.fillRect.GetComponent<Image>().color = Color.gray;
    }

    public void TakeDamage(float damage)
    {
        if (isBallMode)
        {
            
        Debug.Log("Ball enemy took damage: " + damage);
        ball.gameObject.SetActive(false);
        lady.gameObject.SetActive(true);
        isBallMode = false;
        hitbox.radius = 2f;

        healthSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
        else
        {
            currentHealth -= damage;
            healthSlider.value = currentHealth / health;
            if (currentHealth <= 0f)
            {
                Destroy(gameObject);
            }
        
        
        }
        
        // Implement health reduction and death logic here
    }

    public void stopMovement()
    {

    }

    public void resumeMovement()
    {
        player.enableMovement();
    }
    public void destroySelf()
    {
        
    }

    // FixedUpdate is called on fixed physics steps
    void FixedUpdate()
    {
        if (player == null || player.isCutSceneActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > maxDistanceBetweenPlayer)
        {
            UpdateRandomSteerDirection();
            Vector2 toPlayer = (player.transform.position - transform.position).normalized;
            Vector2 separation = GetSeparationDirection();
            Vector2 moveDirection = (toPlayer + separation * separationStrength + randomSteerDirection * randomSteerStrength).normalized;

            rb.linearVelocity = moveDirection * speed;
            return;
        }

        rb.linearVelocity = Vector2.zero;
    }

    Vector2 GetSeparationDirection()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        Vector2 separation = Vector2.zero;

        foreach (Collider2D nearby in nearbyColliders)
        {
            ballenemy otherBallEnemy = nearby.GetComponentInParent<ballenemy>();
            if (otherBallEnemy == null || otherBallEnemy == this)
            {
                continue;
            }

            Vector2 offset = (Vector2)(transform.position - otherBallEnemy.transform.position);
            float distance = offset.magnitude;
            if (distance > 0f)
            {
                separation += offset.normalized / distance;
            }
        }

        return separation;
    }

    void UpdateRandomSteerDirection()
    {
        if (Time.time < nextRandomSteerUpdateTime)
        {
            return;
        }

        Vector2 random = Random.insideUnitCircle;
        if (random.sqrMagnitude < 0.01f)
        {
            random = Vector2.right;
        }

        randomSteerDirection = random.normalized;
        nextRandomSteerUpdateTime = Time.time + randomSteerUpdateInterval;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBallMode)
        {
            player.TakeDamage(20f); 

        }
    }
}
