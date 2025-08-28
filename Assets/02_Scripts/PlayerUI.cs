using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // private 필드(인스펙터 노출)
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text text_CurrentHealth;

    // private 필드(컴포넌트)
    private PlayerStats playerStats;

    // 유니티 콜백
    private void Start()
    {
        PlayerController.Instance.TryGetComponent(out playerStats);
        playerStats.OnHealthChanged += HandleHealthChanged;

        InitHealthUI(); // 시작 시 초기화
    }
    private void OnDisable()
    {
        playerStats.OnHealthChanged -= HandleHealthChanged;
    }

    // 메인
    private void InitHealthUI()
    {
        healthBar.fillAmount = (float)playerStats.CurrentHealth / playerStats.MaxHealth;
        text_CurrentHealth.text = playerStats.CurrentHealth.ToString("F0");
    }
    private void HandleHealthChanged(float current, float max) // 체력이 변할 때 UI 변화
    {
        float currentFill = healthBar.fillAmount;
        float targetFill = current / max;

        // 기존 트윈 종료 후 새 트윈 실행(트윈 진행 중 또 데미지를 입을 때 자연스러운 변화를 위함)
        healthBar.DOKill();
        text_CurrentHealth.DOKill();

        healthBar.DOFillAmount(targetFill, Consts.HEALTH_BAR_TWEEN_DURATION)
                 .From(currentFill)
                 .SetEase(Ease.OutQuad);

        // 텍스트 변경
        float currentValue = float.Parse(text_CurrentHealth.text);
        DOTween.To(() => currentValue, x => text_CurrentHealth.text = Mathf.RoundToInt(x).ToString(), current, Consts.HEALTH_BAR_TWEEN_DURATION)
               .SetEase(Ease.OutQuad);
    }
}
