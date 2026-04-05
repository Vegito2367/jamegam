using UnityEngine;

public class ladyscript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    ballenemy ball;
    

    void Start()
    {
        ball = GetComponentInParent<ballenemy>();
    }
    public void deleteSelf()
    {
        Debug.Log("Lady script self-destructing");
        ball.destroySelf();
    }
    public void stopMovement()
    {
        ball.stopMovement();
    }

}
