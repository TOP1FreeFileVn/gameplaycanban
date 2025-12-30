using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseCharacter
{
    private string enemyTag = "Enemy";
    public Transform firePoint;
    private PlayerState currentState;


    [Header("Keo player data vao day cho toi")]
    public PlayerStatsData playerStatsData;
    public WeaponStats weaponStats;

    private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed = 0f;
    [SerializeField] private float _fireRate = 0f;

    public float fireRate => _fireRate;
    public float bulletSpeed => _bulletSpeed;
    public GameObject bulletPrefab => _bulletPrefab;

    private Coroutine _scanCoroutine;

    [SerializeField] private Transform _currentTarget;
    public Transform currentTarget => _currentTarget;


    protected override void Start()
    {
        if (playerStatsData != null)
        {
            InitStats(playerStatsData.maxHealth, playerStatsData.moveSpeed,playerStatsData.attackSpeed,playerStatsData.attackRange);
        }
        healthBarOffset = 1f; 
        base.Start();
        if (weaponStats != null)
        {
            SetUpWeapon(weaponStats.fireRate, weaponStats.bulletSpeed, weaponStats.bulletPrefab);
        }

        ChangeState(new PlayerIdleState(this));
    }

    void Update()
    {
        if(currentState != null)
        {
            currentState.Tick();
        }
       
    }
    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }
    private void SetUpWeapon(float rate,float bulletsp , GameObject bullet)
    {
        _fireRate = rate;
        _bulletPrefab = bullet;
        _bulletSpeed = bulletsp;
    }
    //Atack 
    public GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }
        return nearestEnemy;
    }
    public void StopScanning()
    {
        if(_scanCoroutine != null)
        {
            StopCoroutine(_scanCoroutine);
            _scanCoroutine = null;
        }
        _currentTarget = null;
    }
    public void StartScanning()
    {
        if(_scanCoroutine != null) StopCoroutine(_scanCoroutine);
        _scanCoroutine = StartCoroutine(ScanEnemyRoutine());
    }
    private IEnumerator ScanEnemyRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            GameObject foundEnemy = FindNearestEnemy();
            if (foundEnemy != null)
            {
                _currentTarget = foundEnemy.transform;
            }
            else
            {
                _currentTarget = null;
            }
            yield return wait;
        }
    }
    public void Shoot(Transform target)
    {
        Vector3 dir = target.position - firePoint.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
        GameObject bulletGo = Instantiate(_bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bulletGo.GetComponent<Rigidbody2D>();
        rb.AddForce(_bulletSpeed * dir.normalized, ForceMode2D.Impulse);
    }
    //Anm
    public void TriggerAttack()
    {
        if(_animator  != null)
        {
            _animator.SetTrigger("Shoot");
        }
    }
    public void AttackSpeed()
    {
        if (_animator != null)
        {
            _animator.SetFloat("AttackSpeed",currentAttackSpeed);
        }
    }
}
