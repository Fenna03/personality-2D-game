using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class moving : MonoBehaviour
{
    //dashing
    [SerializeField] float startDashTime = 0.8f;
    [SerializeField] float dashSpeed = 6f;
    public bool canDash = true;
    float currentDashTime;

    [SerializeField] float startSlideTime = 0.8f;
    [SerializeField] float slideSpeed = 6f;
    public bool canSlide = true;
    float currentSlideTime;

    //speed
    public float speed = 3f;

    //jumping
    private float horizontal;
    public float jumpingPower = 7f; 
    public bool isGrounded = true;
    public int jumpAmount = 0;

    //attacking
    public bool canAttack = true;
    public bool isAttacking = false;
    public float attackCooldown = 1.0f;
    public float attackRange = 2f;

    //flipping
    private bool isFacingRight = true;

    Rigidbody2D rb;
    private Animator animator;
    public GameObject OC;
    //public AudioClip attackSound;
    public new BoxCollider2D collider;
    public new AudioBehaviour audio;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioBehaviour>();
        animator.SetBool("isWalking", false);
        GetComponent<BoxCollider2D>().size = new Vector2(0.1925637f, 0.3401231f);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && canAttack)
        {
            SignAttack();
        }

        if (Input.GetMouseButton(1) && canDash)
        {
            if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(Dash(Vector2.left));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(Dash(Vector2.right));
            }
        }

        if(Input.GetKeyDown(KeyCode.C) && canSlide)
        {
           if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(Slide(Vector2.left));
            }
           else if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(Slide(Vector2.right));
            }
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();
        Jump();
        Crouch();
        Walk();
    }

    bool IsObstacleAbove()
    {
        Vector2 rayOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, 1f, groundLayer);
        return hit.collider != null;
    }
    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpAmount < 1 && !Input.GetKey(KeyCode.LeftShift))
        {
            isGrounded = false;
            canAttack = false;
            jumpAmount++;
            animator.SetBool("isJumping", true);
            animator.SetBool("isWalking", false);
            if (jumpAmount >= 1)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isSecondJumping", true);
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
    }
    void Crouch()
    {
        // Check if something is above the player
        bool obstacleAbove = IsObstacleAbove();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isCrouching", true);
            GetComponent<BoxCollider2D>().size = new Vector2(0.1925637f, 0.2380938f);
            collider.offset = new Vector2(-0.07315239f, -0.09058263f);

        }
        else if (!obstacleAbove)
        {
            // LeftShift is not pressed, or released
            GetComponent<BoxCollider2D>().size = new Vector2(0.1925637f, 0.3401231f);
            animator.SetBool("isCrouching", false);
            collider.offset = new Vector2(-0.07315239f, -0.04058263f);
        }
    }
    void Walk()
    {

        if (Input.GetKey("d") || Input.GetKey("a"))
        {
            if (isGrounded == true)
            {
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
    public void SignAttack()
    {
        isAttacking = true;
        canAttack = false;
        Animator anim = OC.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        audio.GetComponent<AudioSource>().Play();
        StartCoroutine(resetAttackCooldown());
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            jumpAmount = 0;
            isGrounded = true;
            canAttack = true;
            animator.SetBool("isSecondJumping", false);
            animator.SetBool("isJumping", false);
        }
    }
    void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            isGrounded = false;
            canAttack = false;
        }
    }

    IEnumerator resetAttackCooldown()
    {
        StartCoroutine(resetAttackBool());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    IEnumerator resetAttackBool()
    {
        yield return new WaitForSeconds(20.0f);
        isAttacking = false;
    }
    IEnumerator Dash(Vector2 direction)
    {
        canDash = false;
        animator.SetBool("Dash", true);

        currentDashTime = startDashTime; // Reset the dash timer.
        
        while (currentDashTime > 0f)
        {
            
            currentDashTime -= Time.deltaTime; // Lower the dash timer each frame.

            rb.velocity = direction * dashSpeed; // Dash in the direction that was held down.
                                                 // No need to multiply by Time.DeltaTime here, physics are already consistent across different FPS.

            yield return null; // Returns out of the coroutine this frame so we don't hit an infinite loop.
        }

        rb.velocity = new Vector2(0f, 0f); // Stop dashing.

        canDash = true;
        animator.SetBool("Dash", false);
    }

    IEnumerator Slide(Vector2 direction)
    {
        canSlide = false;
        animator.SetBool("isSliding", true);

        currentSlideTime = startSlideTime; // Reset the slide timer.

        while (currentSlideTime > 0f)
        {

            currentSlideTime -= Time.deltaTime; // Lower the slide timer each frame.

            rb.velocity = direction * slideSpeed; // Dash in the direction that was held down.
                                                 // No need to multiply by Time.DeltaTime here, physics are already consistent across different FPS.

            yield return null; // Returns out of the coroutine this frame so we don't hit an infinite loop.
        }

        rb.velocity = new Vector2(0f, 0f); // Stop sliding.

        canSlide = true;
        animator.SetBool("isSliding", false);
    }
}
