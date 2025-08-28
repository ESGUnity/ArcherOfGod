using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : BaseStats
{
    protected override void Awake()
    {
        maxHealth = Consts.PLAYER_BASE_MAX_HEALTH;
        damage = Consts.PLAYER_BASE_DAMAGE;
        attackSpeed = Consts.PLAYER_BASE_ATTACK_SPEED;
        moveSpeed = Consts.PLAYER_BASE_MOVE_SPEED;

        base.Awake();

        StartCoroutine(DamageTestRoutine());
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
    private IEnumerator DamageTestRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f); 
            TakeDamage(100f);              
        }
    }
}
