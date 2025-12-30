using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacter
{
    [Header("Enemy Specific")]
    public EnemyStats enemyStats;
    private EnemyState currentState;

    private Transform player;
    private Transform pos;

    [Header("Boid Settings")]
    public float neighborRadius = 1.5f;
    public float separationForce = 2f;
    public LayerMask enemyLayer;

    public float enemySight = 10f;
    protected override void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (enemyStats != null)
        {
            
            float finalSpeed = enemyStats.moveSpeed;
            if (playerObj != null)
            {
                finalSpeed -= playerObj.GetComponent<PlayerController>().currentSpeed;
            }

            InitStats(enemyStats.baseHealth, finalSpeed,enemyStats.attackRange,enemyStats.attackSpeed);
        }
        healthBarOffset = 2f;
        base.Start();

        ChangeState(new IdleState(this));
    }

    void Update()
    {
        if(currentState != null)
        {
            currentState.Tick();
        }
    }
    public void ChangeState(EnemyState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }


    //State
    public bool IsPlayerInSight()
    {
        return Vector3.Distance(transform.position, player.position) < enemySight;
    }
    public bool IsPlayerInRangeAttack()
    {
        return Vector3.Distance(transform.position, player.position) < currentRange;
    }
    public bool IsPlayerOutRangeAttack()
    {
        return Vector3.Distance(transform.position, player.position) >= currentRange;
    }
    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position,player.position);
    }
    public void MoveTowardsPlayer()
    {
        Vector2 seek = (player.position - transform.position).normalized;
        Vector2 separation = CalculateSeparation();

        Vector2 moveDir =
            seek +
            separation * separationForce;

        moveDir.Normalize();

        transform.position += (Vector3)(moveDir * currentSpeed * Time.deltaTime);
    }
    //Anm
    public void TriggerAttack()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }
    }
    
    public void BoolRun()
    {
        if (_animator != null)
        {
            _animator.SetBool("IsRun", true);
        }
    }
    public void OffBoolRun()
    {
        if (_animator != null)
        {
            _animator.SetBool("IsRun", false);
        }
    }

    private Collider2D[] neighborColliders = new Collider2D[10];

    Vector2 CalculateSeparation()
    {

        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            neighborRadius,
            neighborColliders,
            enemyLayer
        );

        Vector2 separation = Vector2.zero;
        int separationCount = 0;

        for (int i = 0; i < count; i++)
        {
            Collider2D col = neighborColliders[i];

            if (col.gameObject == gameObject) continue;

            Vector2 diff = (Vector2)(transform.position - col.transform.position);
            float dist = diff.sqrMagnitude; 


            if (dist > 0.01f)
            {
           
                separation += diff / dist;
                separationCount++;
            }
        }

        if (separationCount > 0)
        {
            separation /= separationCount;
        }

        return separation;
    }

}
