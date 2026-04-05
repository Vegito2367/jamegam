using Unity.VisualScripting;
using UnityEngine;

public class purpleprojectile : MonoBehaviour
{
    public float damage = 100f;
    public float lifetime = 5f;
    public Transform resetPoint;
    public bool released = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            headman enemy = collision.GetComponent<headman>();
            skullenemy skullenemy = collision.GetComponent<skullenemy>();
            ballenemy ballenemy = collision.GetComponent<ballenemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            else if (skullenemy != null)
            {
                skullenemy.TakeDamage(damage);
            }
            else if (ballenemy != null)
            {
                ballenemy.TakeDamage(damage);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (released)
        {
            lifetime -= Time.deltaTime;
        }
        
        if (lifetime <= 0f)
        {
            transform.position = resetPoint.position;
          
            lifetime = 5f; 
            gameObject.SetActive(false); 
        }
    }
}
