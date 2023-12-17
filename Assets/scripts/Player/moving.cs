using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 5f;
    public float attackRange = 2f;

    public bool grounded;
    public bool canMove = true;

    Rigidbody2D rb;
    private Animator anim;
    public GameObject OC;
    public GameObject enemy;

    public bool canAttack = true;
    public bool isAttacking = false;

    public float attackCooldown = 1.0f;
    public AudioClip attackSound;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();

        if (Input.GetButton("Fire1") && canAttack)
        {
            SignAttack();
        }
    }

    public void Moving()
    {
        if (canMove)
        {
            canMove = true;
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
            {
                if (grounded == true)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    //anim.SetBool("Jumping", true);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                //anim.Play("Run");
                anim.SetBool("IsMoving", true);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                //anim.Play("Run");
                anim.SetBool("IsMoving", true);
            }
            if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                anim.SetBool("IsMoving", false);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                //transform.Translate(Vector2.down * speed * Time.deltaTime);
                anim.Play("Crouch");
            }
        }
        else 
        {
            canMove = false;
            anim.SetBool("IsMoving", false);
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("ground"))
        {
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("ground"))
        {
            grounded = false;
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
