using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Flip : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // store movement from horizontal axis of controller
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(horizontalInput, 0);

        // call function
        DetermineFacing(move);

        // move the character
        MoveCharacter(move);
    }

    // determine direction of character
    void DetermineFacing(Vector2 move)
    {
        if (move.x < -0.01f)
        {
            Flipped(false);
        }
        else if (move.x > 0.01f)
        {
            Flipped(true);
        }
    }

    void MoveCharacter(Vector2 move)
    {
        // move the character
        Vector2 newPosition = rb.position + move * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }

    void Flipped(bool faceRight)
    {
        // flip the character horizontally
        Vector3 newScale = transform.localScale;
        newScale.x = faceRight ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }
}

