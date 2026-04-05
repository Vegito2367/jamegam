using UnityEngine;

public class attack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Player_Script player_Script;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Debug.Log("Player attack collided with enemy: " + collision.name);
            headman enemy = collision.GetComponent<headman>();
            if (enemy != null)
            {
                enemy.TakeDamage(player_Script.attackDamage);
            }
        }
    }
    void Start()
    {
        player_Script = FindAnyObjectByType<Player_Script>();
    }
}
