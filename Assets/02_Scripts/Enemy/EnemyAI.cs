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

    // 상태 변수
    private Coroutine attackRoutine;
    private float moveInput = 0f;
    private bool isAttacking = false;

    // 상수
    private const float forwardY = 180f; // 공격 시 고정 회전값

    // 유니티 콜백
    private void Awake()
    {
        TryGetComponent(out stats);
        TryGetComponent(out rb);
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        StartCoroutine(MoveAndAttackRoutine());
    }
    private void Update()
    {
        healthUICanvas.localRotation = transform.rotation;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * stats.MoveSpeed, rb.linearVelocityY);
    }

    // 메인
    private IEnumerator MoveAndAttackRoutine()
    {
        while (true)
        {
            // 게임이 진행 중일 때만 동작
            while (GameManager.Instance.CurrentState != GameStateEnum.Playing)
            {
                yield return null;
            }

            // 이동
            float moveTime = 0f;
            float moveDuration = Random.Range(Utility.ENEMY_MIN_MOVE_DURATION, Utility.ENEMY_MAX_MOVE_DURATION);
            moveInput = Random.Range(0f, 1f) > 0.5 ? 1f : -1f;

            while (moveTime < moveDuration)
            {
                HandleMovementAnimation();

                moveTime += Time.deltaTime;
                yield return null;
            }

            // 공격
            moveInput = 0;
            isAttacking = true;
            int shots = Random.Range(Utility.ENEMY_MIN_ARROWS_PER_STOP, Utility.ENEMY_MAX_ARROWS_PER_STOP);
            float attackDuration = Mathf.Max(0.1f, stats.AttackSpeed);

            for (int i = 0; i < shots; i++)
            {
                // 공격 시작
                animator.SetTrigger("BasicAttack");
                animator.SetTrigger("BasicAttack");

                // 공격 시 회전 고정
                transform.eulerAngles = new Vector3(0, forwardY, 0);

                // 현재 BasicAttack 클립 길이 가져오기
                AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
                AnimatorClipInfo attackClipInfo = System.Array.Find(clipInfos, c => c.clip.name == "BasicAttack");

                if (attackClipInfo.clip != null)
                {
                    float clipLength = attackClipInfo.clip.length;
                    float multiplier = clipLength / attackDuration;

                    animator.SetFloat("BasicAttackSpeed", multiplier);
                }

                yield return new WaitForSeconds(stats.AttackSpeed);

                ShootArrow();
            }

            isAttacking = false;
        }
    }
    private void HandleMovementAnimation()
    {
        if (isAttacking) return; // 공격 중에는 Idle/Run 덮어쓰기 방지

        if (Mathf.Abs(moveInput) > 0.01f)
        {
            transform.rotation = Quaternion.Euler(0f, moveInput > 0 ? 0f : 180f, 0f);
            animator.SetTrigger("Run");
        }
        else
        {
            animator.SetTrigger("Idle");
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
