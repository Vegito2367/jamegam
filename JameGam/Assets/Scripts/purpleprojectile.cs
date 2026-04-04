using Unity.VisualScripting;
using UnityEngine;

public class purpleprojectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public Transform resetPoint;
    public bool released = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
