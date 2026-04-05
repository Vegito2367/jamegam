using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player_Script : MonoBehaviour
{
    //PRIVATE VARIABLES
    Rigidbody2D rb;
    BoxCollider2D attackHitbox;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Transform upAttackPoint;
    Transform downAttackPoint;
    Transform leftAttackPoint;
    Transform rightAttackPoint;


    //Public and serialized variables
    public float speed = 5f;
    public float projectileSpeed = 10f;
    public bool isAttacking = false;
    public Rigidbody2D HollowProjectile;
    public float specialAttackBar = 100f;
    public GameObject hollowCutSceneScreen;
    public GameObject headmanCutSceneScreen;
    public Slider specialAttackSlider;

    public Animator BlackScreenAnimator;

    public bool isLaunchingSpecialMove = false;
    public Light2D light2D;
    public string currentAnimation = "Idle";
    public float attackDamage = 10f;
    public float health = 100f;

    HashSet<headman> enemiesHitThisSwing = new HashSet<headman>();

    headman headman;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        attackHitbox = transform.Find("AttackObject").GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        upAttackPoint = transform.Find("upstart");
        downAttackPoint = transform.Find("downstart");
        leftAttackPoint = transform.Find("leftstart");
        rightAttackPoint = transform.Find("rightstart");
        specialAttackSlider.value = specialAttackBar;
        attackHitbox.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();

        if (isLaunchingSpecialMove)
        {
            HandleSpecialAttack();
        }
    }

    public void HandleSpecialAttack()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            HollowProjectile.position = upAttackPoint.position;
            HollowProjectile.linearVelocity = Vector2.up * projectileSpeed;
            isLaunchingSpecialMove = false;
            HollowProjectile.GetComponent<purpleprojectile>().released = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            HollowProjectile.position = downAttackPoint.position;
            HollowProjectile.linearVelocity = Vector2.down * projectileSpeed;
            isLaunchingSpecialMove = false;
            HollowProjectile.GetComponent<purpleprojectile>().released = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            HollowProjectile.position = leftAttackPoint.position;
            HollowProjectile.linearVelocity = Vector2.left * projectileSpeed;
            isLaunchingSpecialMove = false;
            HollowProjectile.GetComponent<purpleprojectile>().released = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Logic for the special attack
            HollowProjectile.position = rightAttackPoint.position;
            HollowProjectile.linearVelocity = Vector2.right * projectileSpeed;
            isLaunchingSpecialMove = false;
            HollowProjectile.GetComponent<purpleprojectile>().released = true;
        }


    }

    public void doFadeOut()
    {

        BlackScreenAnimator.SetTrigger("FadeOut");
    }
    public void launchPlayerSpecial()
    {

        isLaunchingSpecialMove = true;
        spriteRenderer.enabled = true;
        HollowProjectile.gameObject.SetActive(true);
        specialAttackSlider.enabled = true;
    }

    public void HandleAttack()
    {
        if (isLaunchingSpecialMove)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
            
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && specialAttackBar >= 30f)

        {
            beginFadeIn("hollow");
            spriteRenderer.enabled = false;
            specialAttackBar -= 30f;
            specialAttackSlider.value = specialAttackBar;
            specialAttackSlider.enabled = false;
        }

    }




    public void beginFadeIn(string cutSceneName="")
    {
        BlackScreenAnimator.SetTrigger("FadeIn");
        currentAnimation = cutSceneName;
    }
    public void currentCutSceneStart(string cutSceneName = "")
    {
        light2D.intensity = 1f;
        switch (cutSceneName)
        {
            case "hollow":
                headmanCutSceneScreen.SetActive(false);
                hollowCutSceneScreen.SetActive(true);
                currentAnimation = "hollow";
                break;
            case "headman":
                hollowCutSceneScreen.SetActive(false);
                headmanCutSceneScreen.SetActive(true);
                currentAnimation = "headman";
                break;
            default:
                break;
        }
    }

    public void currentCutSceneEnd(string cutSceneName = "")
    {
        doFadeOut();
        switch (cutSceneName)
        {
            case "hollow":
                hollowCutSceneScreen.SetActive(false);
                launchPlayerSpecial();
                break;
            case "headman":
                headmanCutSceneScreen.SetActive(false);
                break;
            default:
                break;
        }
        currentAnimation = "Idle";
        Debug.Log("Cutscene ended, launching special move");
        light2D.intensity = 0f;
        

    }
    public void setAttackTrue()
    {
        enemiesHitThisSwing.Clear();
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
        if (isAttacking || isLaunchingSpecialMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        rb.linearVelocity = new Vector2(moveX, moveY).normalized * speed;
    }
}
