using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 10;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void Die()
    {
        //if health is less than 5 kill the enemy
        if (Health <= 0)
        {
            Destroy(gameObject);
            Debug.Log(Health);
        }
        //if there is health take one off
        else
        {
            Health--;
            Debug.Log(Health);
        }
    }
}
