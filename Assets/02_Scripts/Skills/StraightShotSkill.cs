using DG.Tweening;
using System.Collections;
using UnityEngine;

public class StraightShotSkill : Skill
{
    // private 필드
    private PlayerController controller;
    private PlayerStats stats;
    private PlayerSkills skills;

    private void Start()
    {
        controller = PlayerController.Instance;
        stats = controller.GetComponent<PlayerStats>();
        skills = controller.GetComponent<PlayerSkills>();

        cooldown = stats.StraightShotCooldown;
    }

    public override bool UseSkill()
    {
        if (isOnCooldown || skills.IsUsingSkill)
        {
            return false;
        }

        StartCoroutine(StraightShotRoutine());
        StartCoroutine(CooldownRoutine());

        return true;
    }

    private IEnumerator StraightShotRoutine()
    {
        if (skills.IsUsingSkill) yield break;
        skills.IsUsingSkill = true;

        for (int i = 0; i < stats.StraightShotArrowCount; i++) // 스트레이트샷 공격 개수만큼 시전
        {
            Enemy target = EnemyManager.Instance.GetClosestEnemy(controller.transform.position);

            if (target != null)
            {
                yield return new WaitForSeconds(Utility.STRAIGHT_SHOT_ATTACK_SPEED);

                GameObject arrowObj = Instantiate(
                    controller.Prefab_BasicArrowProjectile,
                    controller.FirePoint.position,
                    controller.FirePoint.rotation
                );

                // 목표를 향해 직선 발사
                arrowObj.GetComponent<ArrowProjectile>().LaunchStraight(
                    controller.FirePoint.position,
                    target.transform.position,
                    Utility.ARROW_SPEED,
                    stats.Damage,
                    "Enemy"
                );
            }
        }

        skills.IsUsingSkill = false;
    }
}
