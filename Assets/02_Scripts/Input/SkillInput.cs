using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInput : MonoBehaviour, IPointerClickHandler
{
    // private 필드(인스펙터 노출)
    [SerializeField] private SkillBtnEnum buttonType;

    // private 필드(씬 오브젝트)
    [SerializeField] private Image image_Cover;
    [SerializeField] private TMP_Text text_Cooldown;

    // private 필드
    private Color originColor = new Color(0, 0, 0, 0);
    private Color darkColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
    private PlayerSkills playerSkills;

    // 유니티 콜백
    private void Start()
    {
        playerSkills = PlayerController.Instance.Skills;
        text_Cooldown.gameObject.SetActive(false);
    }
    private void Update()
    {
        float remain = playerSkills.GetSkillRemainTime(buttonType);

        if (remain > 0f)
        {
            // 버튼 어둡게, 텍스트 활성화
            image_Cover.color = darkColor;
            text_Cooldown.gameObject.SetActive(true);
            text_Cooldown.text = Mathf.Ceil(remain).ToString(); // 초 단위로 남은 시간 표시
        }
        else
        {
            // 원래 색, 텍스트 비활성화(스킬 사용가능)
            image_Cover.color = originColor;
            text_Cooldown.gameObject.SetActive(false);
        }
    }

    // 메인
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerController.Instance.Skills.UseSkill(buttonType);
    }
}
