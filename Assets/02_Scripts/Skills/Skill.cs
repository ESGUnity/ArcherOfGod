using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // protected 필드
    protected float cooldown;
    protected bool isOnCooldown = false;
    protected float remainTime = 0f; // 남은 쿨타임

    // public Getter
    public float Cooldown => cooldown;
    public bool IsOnCooldown => isOnCooldown;
    public float RemainTime => remainTime;

    // 메인
    public abstract bool UseSkill();

    protected IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        remainTime = cooldown;

        while (remainTime > 0f)
        {
            // 게임 상태가 Playing일 때만 남은 시간 감소
            if (GameManager.Instance.CurrentState == GameStateEnum.Playing)
            {
                remainTime -= Time.deltaTime;
                if (remainTime < 0f) remainTime = 0f;
            }
            
            yield return null;
        }

        isOnCooldown = false;
    }
}
