using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 2f;
    private float horizontal;
    private float jumpingPower = 6f;

    public bool canAttack = true;
    public bool isAttacking = false;
    private bool isFacingRight = true;
    public bool isGrounded = true;

    public int jumpAmount = 0;

    Rigidbody2D rb;
    private Animator animator;
    public GameObject OC;

    public float attackCooldown = 1.0f;
    public AudioClip attackSound;
    public new BoxCollider2D collider;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", false);
        GetComponent<BoxCollider2D>().size = new Vector2(0.1925637f, 0.3401231f);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && canAttack)
        {
            SignAttack();
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && jumpAmount < 1 && !Input.GetKey(KeyCode.LeftShift))
        {
            isGrounded = false;
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

        Flip();

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isCrouching", true);
            GetComponent<BoxCollider2D>().size = new Vector2(0.1925637f, 0.2380938f);
            collider.offset = new Vector2(-0.07315239f, -0.09058263f);
        }
        else
        {
            GetComponent<BoxCollider2D>().size = new Vector2(0.1925637f, 0.3401231f);
            animator.SetBool("isCrouching", false);
            collider.offset = new Vector2(-0.07315239f, -0.04058263f);

        }
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            jumpAmount = 0;
            isGrounded = true;
            animator.SetBool("isSecondJumping", false);
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            isGrounded = false;
        }
    }
public void SignAttack()
    {
        isAttacking = true;
        canAttack = false;
        Animator anim = OC.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        //AudioSource ac = GetComponent<AudioSource>();
        //ac.PlayOneShot(attackSound);
        StartCoroutine(resetAttackCooldown());
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
}
