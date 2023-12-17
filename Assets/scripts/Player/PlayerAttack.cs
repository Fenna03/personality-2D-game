using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject OC;
    public GameObject enemy;
    public bool canAttack = true;
    public float attackCooldown = 1.0f;
    public AudioClip attackSound;
    public bool isAttacking = false;
    public moving Moving;
    public float attackRange = 2.0f; // Set the attack range as needed

    private void Awake()
    {
        Moving = GetComponent<moving>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && canAttack && IsPlayerCloseEnough())
        {
            SignAttack();
        }
    }

    bool IsPlayerCloseEnough()
    {
        return Vector2.Distance(transform.position, enemy.transform.position) <= attackRange;
    }

    public void SignAttack()
    {
        isAttacking = true;
        canAttack = false;
        Animator anim = OC.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        enemy.GetComponent<enemyHealth>().Die();
        //AudioSource ac = GetComponent<AudioSource>();
        //ac.PlayOneShot(attackSound);
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(20.0f);
        isAttacking = false;
    }
}
