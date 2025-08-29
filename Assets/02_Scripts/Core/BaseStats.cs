using System;
using UnityEditor;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_FloatingDamageText;

    // private 필드
    protected float maxHealth;
    protected float currentHealth;
    protected float damage;
    protected float attackSpeed;
    protected float moveSpeed;

    protected float jumpShotArrowCount;
    protected float jumpShotCooldown;

    protected float straightShotArrowCount;
    protected float straightShotCooldown;

    protected float multiShotArrowCount;
    protected float multiShotCooldown;

    // public Getter
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float MoveSpeed => moveSpeed;

    public float JumpShotArrowCount => jumpShotArrowCount;
    public float JumpShotCooldown => jumpShotCooldown;
    public float StraightShotArrowCount => straightShotArrowCount;
    public float StraightShotCooldown => straightShotCooldown;
    public float MultiShotArrowCount => straightShotArrowCount;
    public float MultiShotCooldown => straightShotCooldown;

    // public 필드
    public event Action<float, float> OnHealthChanged;

    // 유니티 콜백
    protected virtual void Awake()
    {
        InitStats();
    }
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // 메인
    protected virtual void InitStats() // 각종 스탯 초기화
    {

    }
    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Instantiate(prefab_FloatingDamageText).GetComponent<FloatingDamageText>().ShowDamage(amount, transform.position);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
