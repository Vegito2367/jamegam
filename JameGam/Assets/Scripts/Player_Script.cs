using System.Security.Cryptography;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    public Rigidbody2D rb;
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
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("'w' is pressed");
        }
    }
}   
