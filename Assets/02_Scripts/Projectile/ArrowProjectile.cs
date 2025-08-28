using System.Collections;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    // private 필드
    private Vector2 start;
    private Vector2 target;
    private float speed;
    private float arcHeight;
    private bool initialized;

    // 메인
    public void Initialize(Vector2 startPos, Vector2 targetPos, float arrowSpeed) // 화살 생성 시
    {
        start = startPos;
        target = targetPos;
        speed = arrowSpeed;

        float distance = Vector2.Distance(start, target);
        arcHeight = Consts.BASE_ARC_HEIGHT + distance * Consts.ARC_HEIGHT_MULTI;

        initialized = true;
        StartCoroutine(Fly());
    }
    private IEnumerator Fly() // 궤도를 따라 날아가는 코루틴
    {
        float distance = Vector2.Distance(start, target);
        float travelTime = distance / speed;
        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / travelTime);

            // 포물선 위치 계산
            Vector2 currentPos = Vector2.Lerp(start, target, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            transform.position = currentPos;
            yield return null;
        }

        transform.position = target;
        HitTarget();
    }

    private void HitTarget()
    {
        Debug.Log("Arrow hit target!");
        Destroy(gameObject);
    }
}
