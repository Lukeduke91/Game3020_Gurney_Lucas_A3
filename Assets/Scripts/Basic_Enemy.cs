using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Basic_Enemy : MonoBehaviour
{
    public int health = 10;

    //public NavMeshAgent enemy;
    public Transform targetObj;

    [SerializeField]
    private float timer = 5;
    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public Transform objectPoint;
    public float enemySpeed;
    public float upDegree = 1;

    void Update()
    {
        //enemy.SetDestination(targetObj.position);
        shoot();
        Vector3 targetDirection = targetObj.position - spawnPoint.position;
        Vector3 objectDirection = targetObj.position - objectPoint.position;

        float singleStep = enemySpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(spawnPoint.forward, targetDirection + (Vector3.up * upDegree), singleStep, 5.0f);
        Vector3 newObjectDirection = Vector3.RotateTowards(objectPoint.forward, objectDirection + (Vector3.up * upDegree), singleStep, 5.0f);

        Debug.DrawRay(spawnPoint.position, newDirection, Color.red);
        Debug.DrawRay(objectPoint.position, newObjectDirection, Color.red);
        spawnPoint.rotation = Quaternion.LookRotation(newDirection);
        objectPoint.rotation = Quaternion.LookRotation(newObjectDirection);
    }

    public void shoot()
    {
        bulletTime -= Time.deltaTime;
        // B(Players pos) - A(Enemy bullet spawn pos)
        if (bulletTime > 0)
        {
            return;
        }

        bulletTime = timer;

        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * enemySpeed, ForceMode.Impulse);
        Destroy(bulletObj, 2f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Done");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
