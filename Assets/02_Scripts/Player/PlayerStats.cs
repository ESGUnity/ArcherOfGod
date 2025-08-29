using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : BaseStats
{
    // 유니티 콜백
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    // 메인
    protected override void InitStats()
    {
        base.InitStats();
        maxHealth = Utility.PLAYER_BASE_MAX_HEALTH;
        damage = Utility.PLAYER_BASE_DAMAGE;
        attackSpeed = Utility.PLAYER_BASE_ATTACK_SPEED;
        moveSpeed = Utility.PLAYER_BASE_MOVE_SPEED;

        jumpShotArrowCount = Utility.JUMP_SHOT_ARROW_COUNT;
        jumpShotCooldown = Utility.JUMP_SHOT_COOLDOWN;

        straightShotArrowCount = Utility.STRAIGHT_SHOT_ARROW_COUNT;
        straightShotCooldown = Utility.STRAIGHT_SHOT_COOLDOWN;

        multiShotArrowCount = Utility.MULTI_SHOT_ARROW_COUNT;
        multiShotCooldown = Utility.MULTI_SHOT_COOLDOWN;
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
