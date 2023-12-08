using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Add_On : MonoBehaviour
{
    public int damage;

    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(targetHit)
        {
            return;
        }
        else
        {
            targetHit = true;
        }

        if (collision.gameObject.GetComponent<Basic_Enemy>() != null)
        {
            Basic_Enemy enemy = collision.gameObject.GetComponent<Basic_Enemy>();
            
            enemy.TakeDamage(damage);
            
            Destroy(gameObject);
        }
        else if(collision.gameObject.GetComponent<MovingEnemyAI>() != null)
        {
            MovingEnemyAI movingEnemyAI = collision.gameObject.GetComponent<MovingEnemyAI>();
            movingEnemyAI.TakeDamage(damage);

            Destroy(gameObject);
        }

        rb.isKinematic = true;

        transform.SetParent(collision.transform);
        Destroy(gameObject);
    }
}
