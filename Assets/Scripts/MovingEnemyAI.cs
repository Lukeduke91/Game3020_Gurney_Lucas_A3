using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingEnemyAI : MonoBehaviour
{
    public int health = 10;
    public NavMeshAgent enemy;
    public Transform targetObj;

    [SerializeField]
    private float timer = 5;
    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float enemySpeed;

    void Update()
    {
        enemy.SetDestination(targetObj.position);
        ShootAtPlayer(); 
    }

    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if(bulletTime > 0)
        {
            return;
        }

        bulletTime = timer;

        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
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

    //Original Program\\
    //public LayerMask Grounded, isThatPlayer;

    //public Vector3 walkPoint;
    //bool walkPointSet;
    //public float walkPointRange;
    ////Attacking
    //public float timeBetweenAttacks;
    //bool alreadyAttacked;
    //public GameObject projectile;

    ////States
    //public float sightRange, attackRange;
    //public bool playerInSightRange, playerInAttackRange;

    //private void Awake()
    //{
    //    playerLocation = GameObject.Find("PlayerClass").transform;
    //    agent = GetComponent<NavMeshAgent>();
    //}

    //private void Update()
    //{
    //    playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isThatPlayer);
    //    playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isThatPlayer);

    //    if (!playerInSightRange && !playerInAttackRange) 
    //        Patroling();
    //    if (!playerInSightRange && !playerInAttackRange) 
    //        ChasePlayer();
    //    if (!playerInSightRange && !playerInAttackRange) 
    //        AttackPlayer();
    //}

    //private void Patroling()
    //{
    //    if(!walkPointSet)
    //        SearchWalkPoint();

    //    if(walkPointSet)
    //        agent.SetDestination(walkPoint);

    //    Vector3 distanceToWalkPoint = transform.position - walkPoint;

    //    if (distanceToWalkPoint.magnitude < 1f)
    //        walkPointSet = false;
    //}

    //private void SearchWalkPoint()
    //{
    //    float RandomZ = Random.Range(-walkPointRange, walkPointRange);
    //    float RandomX = Random.Range(-walkPointRange, walkPointRange);

    //    walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

    //    if(Physics.Raycast(walkPoint, -transform.up, 2f, Grounded))
    //    {
    //        walkPointSet = true;
    //    }
    //}

    //private void ChasePlayer()
    //{
    //    agent.SetDestination(playerLocation.position);
    //}

    //private void AttackPlayer()
    //{
    //    agent.SetDestination(transform.position);

    //    transform.LookAt(playerLocation);

    //    if(!alreadyAttacked)
    //    {
    //        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

    //        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
    //        rb.AddForce(transform.up * 8f, ForceMode.Impulse);

    //        alreadyAttacked = true;
    //        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //    }
    //}
    //private void ResetAttack()
    //{
    //    alreadyAttacked = false;
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, sightRange);
    //}
}
