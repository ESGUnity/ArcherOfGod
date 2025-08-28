using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : BaseStats
{
    // private 필드(인스펙터 노출)
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text text_CurrentHealth;

    protected override void Start()
    {
        base.Start();

        InitHealthUI(); // 시작 시 초기화
    }

    protected override void InitStats()
    {
        base.InitStats();

        int currentWave = GameManager.Instance.CurrentWave; // 현재 웨이브
        int enemyCount = EnemyManager.Instance.Enemies.Count; // 현재 스폰된 적 수

        // 웨이브별 능력치 계산
        float healthMultiplier = 1 + (ConstsAndEnums.ENEMY_HEALTH_MULTI * (currentWave - 1));
        float damageMultiplier = 1 + (ConstsAndEnums.ENEMY_DAMAGE_MULTI * (currentWave - 1));
        float attackSpeedReduction = ConstsAndEnums.ENEMY_BASE_ATTACK_SPEED
                                     - (Mathf.Floor((currentWave - 1) / 5f) * ConstsAndEnums.ENEMY_ATTACK_SPEED_MULTI);
        attackSpeedReduction = Mathf.Max(0.5f, attackSpeedReduction); // 최소 공격속도 제한

        // 스폰된 적 개수에 따른 보정값 계산
        float balanceMultiplier = 1f;
        switch (enemyCount)
        {
            case 2:
                balanceMultiplier = 0.6f;
                break;
            case 3:
                balanceMultiplier = 0.35f;
                break;
        }

        // 최종 스탯 설정
        maxHealth = ConstsAndEnums.ENEMY_BASE_MAX_HEALTH * healthMultiplier * balanceMultiplier;
        damage = ConstsAndEnums.ENEMY_BASE_DAMAGE * damageMultiplier * balanceMultiplier;
        attackSpeed = attackSpeedReduction;
        moveSpeed = ConstsAndEnums.ENEMY_BASE_MOVE_SPEED;

        Debug.Log($"Wave {currentWave} | EnemyCount: {enemyCount} | HP: {maxHealth}, DMG: {damage}, ASPD: {attackSpeed}, MOVE: {moveSpeed}");
    }
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        HandleHealthChanged();
    }
    protected override void Die()
    {
        base.Die();
    }

    // UI
    private void InitHealthUI()
    {
        healthBar.fillAmount = CurrentHealth / MaxHealth;
        text_CurrentHealth.text = CurrentHealth.ToString("F0");
    }
    private void HandleHealthChanged() // 체력이 변할 때 UI 변화
    {
        float currentFill = healthBar.fillAmount;
        float targetFill = CurrentHealth / MaxHealth;

        // 기존 트윈 종료 후 새 트윈 실행(트윈 진행 중 또 데미지를 입을 때 자연스러운 변화를 위함)
        healthBar.DOKill();
        text_CurrentHealth.DOKill();

        healthBar.DOFillAmount(targetFill, ConstsAndEnums.HEALTH_BAR_TWEEN_DURATION)
                 .From(currentFill)
                 .SetEase(Ease.OutQuad);

        // 텍스트 변경
        float currentValue = float.Parse(text_CurrentHealth.text);
        DOTween.To(() => currentValue, x => text_CurrentHealth.text = Mathf.RoundToInt(x).ToString(), CurrentHealth, ConstsAndEnums.HEALTH_BAR_TWEEN_DURATION)
               .SetEase(Ease.OutQuad);
    }
}
