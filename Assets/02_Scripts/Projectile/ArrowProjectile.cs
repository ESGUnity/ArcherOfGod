using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_FireVFX;
    [SerializeField] private GameObject prefab_HitVFX;

    // private 필드
    private Vector3 startPos;
    private Vector3 targetPos;
    private float arcHeight;
    private float speed;
    private float travelTime;
    private float damage;
    private string targetTag;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
    }

    // 메인
    public void LaunchArc(Vector3 start, Vector3 target, float arcHeight, float speed, float damage, string targetTag)
    {
        // 주요 변수 초기화
        startPos = start;
        targetPos = target;
        targetPos.y = -0.3f;
        this.arcHeight = arcHeight;
        this.speed = speed;
        this.damage = damage;
        this.targetTag = targetTag;

        float distance = Vector3.Distance(start, target);
        travelTime = distance / speed * 1.2f;

        // 회전 처리
        Vector3 prevPos = startPos;
        Vector3 position = Vector3.Lerp(startPos, targetPos, 0.05f);
        position.y += arcHeight * Mathf.Sin(0);
        transform.position = position;
        Vector3 dir = position - prevPos;
        if (dir.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // 효과
        GameObject go = Instantiate(prefab_FireVFX);
        go.transform.position = transform.position;
        AudioManager.Instance.PlaySFX(SFXEnum.FireArrow);

        // 루틴 시작
        StartCoroutine(MoveAlongArc());
    }
    private IEnumerator MoveAlongArc()
    {
        float elapsed = 0f;
        Vector3 prevPos = startPos;

        while (elapsed < travelTime)
        {
            float t = elapsed / travelTime;
            Vector3 position = Vector3.Lerp(startPos, targetPos, t);
            position.y += arcHeight * Mathf.Sin(Mathf.PI * t);
            transform.position = position;

            // 회전 처리: 이동 방향을 바라보도록
            Vector3 dir = position - prevPos;
            if (dir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            prevPos = position;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        FadeOutAndDestroy();
    }
    public void LaunchStraight(Vector3 start, Vector3 target, float speed, float damage, string targetTag)
    {
        // 변수 초기화
        startPos = start;
        targetPos = target;
        this.speed = speed;
        this.damage = damage;
        this.targetTag = targetTag;

        // 방향 계산
        Vector3 dir = (targetPos - startPos).normalized;

        // 초기 위치 & 회전 세팅
        transform.position = startPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 효과
        GameObject go = Instantiate(prefab_FireVFX);
        go.transform.position = transform.position;
        AudioManager.Instance.PlaySFX(SFXEnum.FireArrow);

        // 루틴 시작
        StartCoroutine(MoveStraight(dir));
    }

    private IEnumerator MoveStraight(Vector3 direction)
    {
        float lifetime = 5f; // 5초 후 강제 삭제 (원하는 값으로 조정 가능)
        float elapsed = 0f;

        while (elapsed < lifetime)
        {
            // 직선 이동
            transform.position += direction * speed * Time.deltaTime;

            elapsed += Time.deltaTime;
            yield return null;
        }

        FadeOutAndDestroy();
    }
    private void FadeOutAndDestroy()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.DOFade(0f, 1.5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 물리 콜백
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            StopAllCoroutines();
            FadeOutAndDestroy();
            GetComponent<Collider2D>().enabled = false;
        }
        else if (collision.CompareTag(targetTag))
        {
            if (targetTag == "Player") // 플레이어인 경우 피격 효과 
            {
                DamagePostEffect.Instance.PlayDamageEffect();
            }

            // 효과
            GameObject go = Instantiate(prefab_HitVFX);
            go.transform.position = transform.position;
            AudioManager.Instance.PlaySFX(SFXEnum.Hit);

            BaseStats stats = collision.GetComponent<BaseStats>();
            stats?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
