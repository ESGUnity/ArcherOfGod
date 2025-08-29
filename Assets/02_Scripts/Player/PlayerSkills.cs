using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    // 상수
    private const float JUMP_HEIGHT_Y_VALUE = 3f;
    private const float JUMP_UP_AND_FALL_TIME = 0.3f;
    private const float JUMP_SHOT_ATTACK_SPEED = 0.2f;

    // private 필드(인스펙터 노출)
    [SerializeField] List<Skill> ownedSkills = new();

    // private 필드(컴포넌트)
    private PlayerController controller;
    private PlayerStats stats;

    // private 필드
    private Vector3 originPos;

    // public 필드
    [HideInInspector] public bool IsUsingSkill;

    // 유니티 콜백
    private void Awake()
    {
        TryGetComponent(out controller);
        TryGetComponent(out stats);
    }

    // 메인
    public bool UseSkill(SkillBtnEnum skill) // 스킬 사용 성공 시 true, 아니면 false 반환
    {
        if (GameManager.Instance.CurrentState != GameStateEnum.Playing) return false;

        return ownedSkills[(int)skill].UseSkill();
    }
    public float GetSkillRemainTime(SkillBtnEnum skill)
    {
        return ownedSkills[(int)skill].RemainTime;
    }
}
