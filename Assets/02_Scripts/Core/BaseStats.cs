using System;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    // private 필드
    protected float maxHealth;
    protected float currentHealth;
    protected float damage;
    protected float attackSpeed;
    protected float moveSpeed;

    // public Getter
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float MoveSpeed => moveSpeed;

    // public 필드
    public event Action<float, float> OnHealthChanged;

    // 유니티 콜백
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    // 메인
    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

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
