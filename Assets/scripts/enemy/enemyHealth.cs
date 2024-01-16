using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 10;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void Die()
    {
        //if health is less than 5 kill the enemy
        if (Health <= 1)
        {
            anim.Play("Death-NoEffect");
            anim.SetBool("isWalking", false);
            StartCoroutine(died());
            Debug.Log(Health);
        }
        //if there is health take one off
        else
        {
            anim.Play("Hurt-NoEffect");
            Health--;
            Debug.Log(Health);
        }
    }

    IEnumerator died()
    {
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}
