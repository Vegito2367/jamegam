using UnityEngine;
using System.Collections.Generic;

public class attack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Player_Script player_Script;
    HashSet<object> hitEnemiesThisSwing = new HashSet<object>();

    void Update()
    {
        if (player_Script != null && !player_Script.isAttacking)
        {
            hitEnemiesThisSwing.Clear();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (player_Script == null || !player_Script.isAttacking)
        {
            return;
        }

        if (collision.CompareTag("enemy"))
        {
            Debug.Log("Player attack collided with enemy: " + collision.name);
            headman enemy = collision.GetComponent<headman>() ?? collision.GetComponentInParent<headman>();
            skullenemy skullenemy = collision.GetComponent<skullenemy>() ?? collision.GetComponentInParent<skullenemy>();
            ballenemy ballenemy = collision.GetComponent<ballenemy>() ?? collision.GetComponentInParent<ballenemy>();

            if (enemy != null)
            {
                if (hitEnemiesThisSwing.Contains(enemy))
                {
                    return;
                }

                enemy.TakeDamage(player_Script.attackDamage);
                hitEnemiesThisSwing.Add(enemy);
            }
            else if (skullenemy != null)
            {
                if (hitEnemiesThisSwing.Contains(skullenemy))
                {
                    return;
                }

                skullenemy.TakeDamage(player_Script.attackDamage);
                hitEnemiesThisSwing.Add(skullenemy);
            }
            else if (ballenemy != null)
            {
                if (hitEnemiesThisSwing.Contains(ballenemy))
                {
                    return;
                }

                ballenemy.TakeDamage(player_Script.attackDamage);
                hitEnemiesThisSwing.Add(ballenemy);
            }
            else
            {
                return;
            }

            player_Script.updateSpecialBar(10f);
        }
    }
    void Start()
    {
        player_Script = FindAnyObjectByType<Player_Script>();
    }
}
