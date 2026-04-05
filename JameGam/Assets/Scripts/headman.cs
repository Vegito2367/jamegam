using UnityEngine;

public class headman : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Player_Script playerScript;
    Rigidbody2D rb;

    public float speed = 5f;
    public float health = 20f;
    public float threshold = 2f;

    bool hasStartedCutScene = false;

    public void startCutScene()
    {
        playerScript.beginFadeIn("headman");
    }



    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerScript = FindAnyObjectByType<Player_Script>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasStartedCutScene && Vector2.Distance(transform.position, playerScript.transform.position) <= threshold)
        {
            startCutScene();
            hasStartedCutScene = true;
            rb.linearVelocity = Vector2.zero; // Stop movement when cutscene starts
        }

        if (playerScript.currentAnimation!="Idle")
        {
            rb.linearVelocity = Vector2.zero; // Stop movement during cutscene
            return;
        }
        rb.linearVelocity = (playerScript.transform.position - transform.position).normalized * speed;
    }
}
