using UnityEngine;
using UnityEngine.Rendering;

public class Player_Script : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D attackHitbox;
    public float speed = 5f;
    public Animator animator;
    public bool isAttacking = false;
    public SpriteRenderer spriteRenderer;

    public float specialAttackBar = 100f;
    public GameObject hollowCutSceneScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           animator = GetComponent<Animator>();
           attackHitbox = GetComponent<BoxCollider2D>();
           rb = GetComponent<Rigidbody2D>();
           spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
        
    }
    public void launchPlayerSpecial()
    {
        hollowCutSceneScreen.SetActive(false);
        spriteRenderer.enabled = true;
        animator.SetTrigger("special");
    }

    public void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && specialAttackBar >= 30f)
        {
            hollowCutSceneScreen.SetActive(true);
            spriteRenderer.enabled = false;
            specialAttackBar -= 30f; 
        }
        
    }
    public void setAttackTrue()
    {
        attackHitbox.enabled = true;
        isAttacking = true;
    }
    public void setAttackFalse()
    {
        attackHitbox.enabled = false;
        isAttacking = false;
    }

    public void HandleMovement()
    {
        if(isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            animator.SetBool("isMoving",true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        rb.linearVelocity = new Vector2(moveX, moveY).normalized * speed;
    }
}   
