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
    [SerializeField] private Transform healthUICanvas;

    // 컴포넌트
    private EnemyStats stats;
    private Rigidbody2D rb;
    private Animator animator;

    // 필드
    private Coroutine attackRoutine;
    private float moveDirection = 1f;

    // 상수
    private const float forwardY = 180f; // 공격 시 바라볼 방향

    private void Awake()
    {
        TryGetComponent(out stats);
        TryGetComponent(out rb);
        animator = GetComponentInChildren<Animator>(); // 자식 오브젝트에서 Animator 가져오기
    }

    private void Start()
    {
        StartCoroutine(MoveAndAttackRoutine());
    }
    private void Update()
    {
        healthUICanvas.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0); // 급하게 체력 UI 수정
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

            // ===== 이동 =====
            float moveTime = 0f;
            float moveDuration = Random.Range(Utility.ENEMY_MIN_MOVE_DURATION, Utility.ENEMY_MAX_MOVE_DURATION);

            animator.SetTrigger("Run");

            while (moveTime < moveDuration)
            {
                rb.linearVelocity = new Vector2(moveDirection * stats.MoveSpeed, rb.linearVelocity.y);
                moveTime += Time.deltaTime;
                yield return null;
            }

            // 정지
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("Idle");

            // ===== 공격 =====
            int shots = Random.Range(Utility.ENEMY_MIN_ARROWS_PER_STOP, Utility.ENEMY_MAX_ARROWS_PER_STOP);
            for (int i = 0; i < shots; i++)
            {
                yield return new WaitForSeconds(stats.AttackSpeed);

                // 공격 시작 애니메이션
                animator.SetTrigger("BasicAttack");

                // **공격할 때만** Y회전을 forwardY로 고정
                transform.eulerAngles = new Vector3(0, forwardY, 0);

                ShootArrow();
            }

            // 방향 전환 (이동 방향만 변경)
            float randomDir = Random.Range(0f, 1f);
            moveDirection = randomDir > 0.5f ? -1 : 1;
        }
    }

    private void ShootArrow()
    {
        PlayerController target = PlayerController.Instance;

        if (target != null)
        {
            float distance = Vector3.Distance(firePoint.position, target.transform.position);
            float arcHeight = Utility.BASE_ARC_HEIGHT + distance * Utility.ARC_HEIGHT_MULTI;

            GameObject arrowObj = Instantiate(prefab_ArrowProjectile, firePoint.position, firePoint.rotation);
            arrowObj.GetComponent<ArrowProjectile>().LaunchArc(
                firePoint.position,
                target.transform.position,
                arcHeight,
                Utility.ARROW_SPEED,
                stats.Damage,
                "Player"
            );
        }
    }
}
