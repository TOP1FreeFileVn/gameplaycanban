using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    [Header("Base Stats")]
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _currentSpeed;
    [SerializeField] private float _currentRange;
    [SerializeField] private float _currentAttackSpeed;
    [Header("Base UI")]
    public GameObject healthBarPrefab;
    protected HealBar healthBar;
    [SerializeField] protected float healthBarOffset = 2f;

    [Header("Animation")]
    protected Animator _animator;

    public float currentRange => _currentRange;
    public float currentHealth => _currentHealth;
    public float currentSpeed => _currentSpeed;
    public float currentAttackSpeed => _currentAttackSpeed;
    public Animator animator => _animator;

    protected virtual void Start()
    {
        InitHealthBar();
        _animator = GetComponentInChildren<Animator>();
    }

    protected void InitHealthBar()
    {
        if (healthBarPrefab != null)
        {
            GameObject healthBarGo = Instantiate(healthBarPrefab, transform.position + Vector3.up * healthBarOffset, Quaternion.identity, transform);
            healthBar = healthBarGo.GetComponent<HealBar>();
            if (healthBar != null) healthBar.SetMaxHealth();
        }
    }

    protected void InitStats(float maxHp, float speed,float aS,float range)
    {
        _maxHealth = maxHp;
        _currentHealth = maxHp;
        _currentSpeed = speed;
        _currentRange = range;
        _currentAttackSpeed = aS;

    }


    public virtual void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;

        if (healthBar != null)
        {
            healthBar.UpdateHealth(_currentHealth, _maxHealth);
        }

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}