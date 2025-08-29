using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MultiShotSkill : Skill
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

        cooldown = stats.MultiShotCooldown; // MULTI_SHOT 쿨다운
    }

    public override bool UseSkill()
    {
        if (isOnCooldown || skills.IsUsingSkill)
        {
            return false;
        }

        StartCoroutine(MultiShotRoutine());
        StartCoroutine(CooldownRoutine());

        return true;
    }

    private IEnumerator MultiShotRoutine()
    {
        if (skills.IsUsingSkill) yield break;
        skills.IsUsingSkill = true;

        Enemy target = EnemyManager.Instance.GetClosestEnemy(controller.transform.position);
        if (target != null)
        {
            yield return new WaitForSeconds(Utility.MULTI_SHOT_ATTACK_SPEED);

            for (int i = -2; i <= 2; i++) // x축 오프셋 -2, -1, 0, 1, 2
            {

                float distance = Vector3.Distance(controller.FirePoint.position, target.transform.position);
                float arcHeight = Utility.BASE_ARC_HEIGHT + distance * Utility.ARC_HEIGHT_MULTI;
                Vector3 targetPos = target.transform.position + new Vector3(i, 0f, 0f);

                GameObject arrowObj = Instantiate(controller.Prefab_BasicArrowProjectile, controller.FirePoint.position, controller.FirePoint.rotation);

                arrowObj.GetComponent<ArrowProjectile>().LaunchArc(
                    controller.FirePoint.position,
                    targetPos,
                    arcHeight,
                    Utility.ARROW_SPEED,
                    stats.Damage,
                    "Enemy"
                );
            }
        }

        skills.IsUsingSkill = false;
    }
}
