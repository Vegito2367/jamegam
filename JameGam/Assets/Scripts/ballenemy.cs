using UnityEngine;
using System.Collections;

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

    bool isBallMode = true;

    void Start()
    {
        ball = transform.Find("Ball");
        lady = transform.Find("Lady");
        player = FindAnyObjectByType<Player_Script>();    
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CircleCollider2D>();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Ball enemy took damage: " + damage);
        ball.gameObject.SetActive(false);
        lady.gameObject.SetActive(true);
        isBallMode = false;
        hitbox.radius = 2f;
        // Implement health reduction and death logic here
    }

    public void stopMovement()
    {
        player.disableMovement();
    }

    public void resumeMovement()
    {
        player.enableMovement();
    }
    public void destroySelf()
    {
        StartCoroutine(DestroySelfAfterDelay());
    }

    private IEnumerator DestroySelfAfterDelay()
    {
        player.enableMovement();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    // FixedUpdate is called on fixed physics steps
    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > maxDistanceBetweenPlayer)
        {
            rb.linearVelocity = (player.transform.position - transform.position).normalized * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBallMode)
        {
            player.TakeDamage(20f); 

        }
    }
}
