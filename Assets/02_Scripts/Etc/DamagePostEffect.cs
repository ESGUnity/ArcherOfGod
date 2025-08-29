using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class DamagePostEffect : MonoBehaviour
{
    // 상수
    private const float MAX_INTENSITY = 0.5f; // 최대 강도
    private const float FADE_IN_TIME = 0.1f;  // 깜빡일 때 올라가는 시간
    private const float FADE_OUT_TIME = 0.5f; // 서서히 사라지는 시간

    private const float SLOW_TIME_SCALE = 0.23f; // 피격 시 슬로우 모션 타임스케일
    private const float SLOW_DURATION = 0.2f; // 슬로우 유지 시간
    private const float TIME_RECOVER_DURATION = 0.3f; // 원래 속도로 복귀하는 시간

    // private 필드(컴포넌트)
    private Volume postProcessVolume;

    // private 필드
    private Vignette vignette;

    // 싱글턴
    private static DamagePostEffect instance;
    public static DamagePostEffect Instance => instance;

    // 유니티 콜백
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        TryGetComponent(out postProcessVolume);
    }
    private void Start()
    {
        if (postProcessVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f; // 시작 시 초기화
        }
        else
        {
            Debug.LogError("Vignette 없음");
        }
    }

    // 메인
    public void PlayDamageEffect()
    {
        // 기존 트윈 제거
        DOTween.Kill("VignetteTween");
        DOTween.Kill("TimeScaleTween");

        vignette.intensity.value = 0f;

        // Vignette 연출
        DOTween.To(() => vignette.intensity.value,
                   x => vignette.intensity.value = x,
                   MAX_INTENSITY,
                   FADE_IN_TIME)
               .SetId("VignetteTween") // 트윈 고유 ID
               .OnComplete(() =>
               {
                   DOTween.To(() => vignette.intensity.value,
                              x => vignette.intensity.value = x,
                              0f,
                              FADE_OUT_TIME)
                           .SetId("VignetteTween");
               });

        // TimeScale 연출
        DOTween.To(() => Time.timeScale,
                   x => Time.timeScale = x,
                   SLOW_TIME_SCALE,
                   0.05f)
               .SetId("TimeScaleTween")
               .OnComplete(() =>
               {
                   DOVirtual.DelayedCall(SLOW_DURATION, () =>
                   {
                       DOTween.To(() => Time.timeScale,
                                  x => Time.timeScale = x,
                                  1f,
                                  TIME_RECOVER_DURATION)
                              .SetId("TimeScaleTween");
                   });
               });
    }
}