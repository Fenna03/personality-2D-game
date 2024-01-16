using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMoving : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    public Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    private float horizontal;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 3f;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Wall>())
        {
            dirX *= -1f;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        anim.SetBool("isWalking", true);
    }

    private void LateUpdate()
    {
       CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
        {
            facingRight = false;
        }
        else if (dirX < 0)
        {
            facingRight= true;
        }

        if (((facingRight) && (localScale.x < 0f)) || (!facingRight) && (localScale.x > 0) )
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }
}
