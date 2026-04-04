using UnityEngine;

public class Player_Script : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        
        
    }

    public void HandleMovement()
    {
        
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(moveX, moveY).normalized * speed;
    }
}   
