using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    // 상수
    private const float FLOAT_DISTANCE = 0.5f;
    private const float FLOAT_DURATION = 1f;

    // private 필드
    private TMP_Text text;

    // 유니티 콜백
    private void Awake()
    {
        TryGetComponent(out text);

        text.GetComponent<Renderer>().sortingLayerName = "UI";
    }

    public void ShowDamage(float damage, Vector3 pos)
    {
        text.text = damage.ToString("F0");
        text.alpha = 1f;

        Vector3 startPos = pos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0, 0.5f), 0);
        Vector3 endPos = startPos + Vector3.up * FLOAT_DISTANCE;

        transform.position = startPos;

        // 이동 & 투명도
        transform.DOMove(endPos, FLOAT_DURATION).SetEase(Ease.OutCubic);
        text.DOFade(0f, FLOAT_DURATION).SetEase(Ease.InCubic).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
