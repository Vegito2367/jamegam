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
            skullenemy skullenemy = collision.GetComponent<skullenemy>();
            ballenemy ballenemy = collision.GetComponent<ballenemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(player_Script.attackDamage);
            }
            else if (skullenemy != null)
            {
                skullenemy.TakeDamage(player_Script.attackDamage);
            }
            else if (ballenemy != null)
            {
                ballenemy.TakeDamage(player_Script.attackDamage);
            }
            player_Script.updateSpecialBar(10f);
        }
    }
    void Start()
    {
        player_Script = FindAnyObjectByType<Player_Script>();
    }
}
