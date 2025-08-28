using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : BaseStats
{
    // 유니티 콜백
    protected override void Start()
    {
        base.Start();
    }

    // 메인
    protected override void InitStats()
    {
        base.InitStats();
        maxHealth = ConstsAndEnums.PLAYER_BASE_MAX_HEALTH;
        damage = ConstsAndEnums.PLAYER_BASE_DAMAGE;
        attackSpeed = ConstsAndEnums.PLAYER_BASE_ATTACK_SPEED;
        moveSpeed = ConstsAndEnums.PLAYER_BASE_MOVE_SPEED;
    }
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
    protected override void Die()
    {
        base.Die();
    }   
}
