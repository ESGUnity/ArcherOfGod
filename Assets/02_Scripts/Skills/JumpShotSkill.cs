using DG.Tweening;
using System.Collections;
using UnityEngine;

public class JumpShotSkill : Skill
{
    // private 필드
    private PlayerController controller;
    private PlayerStats stats;
    private PlayerSkills skills;
    private Vector3 originPos;

    private void Start()
    {
        controller = PlayerController.Instance;
        stats = controller.GetComponent<PlayerStats>();
        skills = controller.GetComponent<PlayerSkills>();

        cooldown = stats.JumpShotCooldown;
    }

    public override bool UseSkill()
    {
        if (isOnCooldown || skills.IsUsingSkill) 
        {
            return false;
        }

        StartCoroutine(JumpShotRoutine());
        StartCoroutine(CooldownRoutine());

        return true;
    }

    private IEnumerator JumpShotRoutine()
    {
        if (skills.IsUsingSkill) yield break;
        skills.IsUsingSkill = true;

        originPos = controller.transform.position;

        // 물리 작용 제거
        Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // 점프
        Vector3 targetHeight = new Vector3(originPos.x, Utility.JUMP_HEIGHT_Y_VALUE, originPos.z);
        controller.transform.DOMove(targetHeight, Utility.JUMP_UP_AND_FALL_TIME).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(Utility.JUMP_UP_AND_FALL_TIME);

        // 공격
        yield return new WaitForSeconds(0.05f); // 체공 직후 약간의 텀

        for (int i = 0; i < stats.JumpShotArrowCount; i++) // 점프샷 공격 개수만큼 시전
        {
            Enemy target = EnemyManager.Instance.GetClosestEnemy(transform.position);

            if (target != null)
            {
                float distance = Vector3.Distance(controller.FirePoint.position, target.transform.position);
                float arcHeight = Utility.BASE_ARC_HEIGHT + distance * Utility.ARC_HEIGHT_MULTI;

                GameObject arrowObj = Instantiate(controller.Prefab_BasicArrowProjectile, controller.FirePoint.position, controller.FirePoint.rotation);
                arrowObj.GetComponent<ArrowProjectile>().LaunchStraight(controller.FirePoint.position, target.transform.position, Utility.ARROW_SPEED, stats.Damage, "Enemy");

                yield return new WaitForSeconds(Utility.JUMP_SHOT_ATTACK_SPEED);
            }
        }

        // 착지
        transform.DOMove(originPos, Utility.JUMP_UP_AND_FALL_TIME).SetEase(Ease.OutQuad);

        skills.IsUsingSkill = false;

        // 물리 작용 재적용
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
