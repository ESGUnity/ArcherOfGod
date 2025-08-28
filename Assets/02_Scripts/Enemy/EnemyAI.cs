using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_ArrowProjectile;

    // 씬 오브젝트
    [Header("씬 오브젝트")]
    [SerializeField] private Transform firePoint;

    // private 필드(컴포넌트)
    private EnemyStats stats;
    private Rigidbody2D rb;

    // private 필드
    private Coroutine attackRoutine;
    private float moveDirection = 1f;

    // 유니티 콜백
    private void Awake()
    {
        TryGetComponent(out stats);
        TryGetComponent(out rb);
    }
    private void Start()
    {
        StartCoroutine(MoveAndAttackRoutine());
    }

    // 이동 및 공격
    private IEnumerator MoveAndAttackRoutine()
    {
        while (true)
        {
            // Playing 상태가 될 때까지 대기
            while (GameManager.Instance.CurrentState != GameStateEnum.Playing)
            {
                yield return null;
            }

            // 이동
            float moveTime = 0f;
            float moveDuration = Random.Range(ConstsAndEnums.ENEMY_MIN_MOVE_DURATION, ConstsAndEnums.ENEMY_MAX_MOVE_DURATION);
            while (moveTime < moveDuration)
            {
                rb.linearVelocity = new Vector2(moveDirection * stats.MoveSpeed, rb.linearVelocity.y);
                moveTime += Time.deltaTime;
                yield return null;
            }

            // 정지
            rb.linearVelocity = Vector2.zero;

            // 정지 후 화살 쏘기
            int shots = Random.Range(ConstsAndEnums.ENEMY_MIN_ARROWS_PER_STOP, ConstsAndEnums.ENEMY_MAX_ARROWS_PER_STOP);
            for (int i = 0; i < shots; i++)
            {
                yield return new WaitForSeconds(stats.AttackSpeed);
                ShootArrow();
            }

            // 방향 전환
            float randomDir = Random.Range(0f, 1f);
            moveDirection = randomDir > 0.5f ? -1 : 1;
        }
    }
    private void ShootArrow()
    {
        Instantiate(prefab_ArrowProjectile, firePoint.position, firePoint.rotation);
    }
}
