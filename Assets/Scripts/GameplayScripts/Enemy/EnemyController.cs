using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyState currentState;
    [Header("Keo enemy data vao day cho toi")]
    public EnemyStats enemyStats;
    private GameObject playerGameObject;
    private Transform player;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _currentRange;
    
    public float currentHealth => _currentHealth;
    public float currentSpeed => _currentSpeed;
    public float currentRange => _currentRange;

    
    public float enemySight = 10f;

    public GameObject healthBarPrefab;
    private HealBar healthBar;
    private float _maxHealth;
    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if (playerGameObject != null) 
        {
            player = playerGameObject.transform;
        }
        
        if(healthBarPrefab != null)
        {
            GameObject healthBarGo = Instantiate(healthBarPrefab,transform.position + Vector3.up * 2f,Quaternion.identity,transform);
            healthBar = healthBarGo.GetComponent<HealBar>();
        }
        if(enemyStats != null)
        {
            SetUpStats(enemyStats.baseHealth, enemyStats.moveSpeed, enemyStats.rangeAttack);
            _maxHealth = enemyStats.baseHealth;
            if(healthBar != null)
            {
                healthBar.SetMaxHealth();
            }
        }

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


    public void TakeDamage(float damageAmount)
    {

        _currentHealth -= damageAmount;
        if(healthBar != null)
        {
            healthBar.UpdateHealth(_currentHealth, _maxHealth);
        }
        if(_currentHealth < 0)
        {
            _currentHealth = 0;
            Die();
        }
    }
    public void Die()
    {
        if(healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }
        Destroy(gameObject);
    }
    //Data
    private void SetUpStats(float hp,float speed,float range)
    {
        _currentHealth = hp;
        _currentSpeed = speed - playerGameObject.GetComponent<PlayerController>().currentSpeed;
        _currentRange = range;
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
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime *  currentSpeed);
    }

}
