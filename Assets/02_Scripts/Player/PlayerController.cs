using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_BasicArrowProjectile;

    // 씬 오브젝트
    [Header("씬 오브젝트")]
    [SerializeField] private Transform firePoint;

    // 컴포넌트
    private Rigidbody2D rb;
    private PlayerStats stats;
    private PlayerSkills skills;
    private Animator animator;

    // 이동
    private float moveInput;
    private Coroutine attackRoutine;

    // 공격 상태
    private bool isAttacking = false;
    private float forwardY = 0;

    // public Getter
    public Transform FirePoint => firePoint;
    public GameObject Prefab_BasicArrowProjectile => prefab_BasicArrowProjectile;
    public PlayerSkills Skills => skills;

    // 싱글턴
    private static PlayerController instance;
    public static PlayerController Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        TryGetComponent(out rb);
        TryGetComponent(out stats);
        TryGetComponent(out skills);
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        // moveInput = Input.GetAxisRaw("Horizontal"); // PC 입력
#endif
        HandleMovementAnimation();

        // 이동하지 않고, 웨이브 진행 중이며, 스킬 사용 중이 아니면 공격 루프 시작
        if (Mathf.Abs(moveInput) < 0.01f && GameManager.Instance.CurrentState == GameStateEnum.Playing && !skills.IsUsingSkill)
        {
            if (attackRoutine == null)
            {
                attackRoutine = StartCoroutine(AttackLoop());
            }
        }
        else
        {
            StopAttack();
        }
    }

    private void FixedUpdate()
    {
        if (skills.IsUsingSkill) moveInput = 0;

        rb.linearVelocity = new Vector2(moveInput * stats.MoveSpeed, rb.linearVelocityY);
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

    // 이동
    public void MoveLeft() => moveInput = -1f;
    public void MoveRight() => moveInput = 1f;
    public void StopMove() => moveInput = 0f;

    // 공격
    private IEnumerator AttackLoop()
    {
        while (true)
        {
            float attackDuration = Mathf.Max(0.1f, stats.AttackSpeed);
            float elapsed = 0f;

            // 공격 시작
            isAttacking = true;
            animator.SetTrigger("BasicAttack");

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

            // 공격 지속 시간 동안 상태 체크
            while (elapsed < attackDuration)
            {
                if (Mathf.Abs(moveInput) > 0.01f || GameManager.Instance.CurrentState != GameStateEnum.Playing || skills.IsUsingSkill)
                {
                    // 공격 취소
                    isAttacking = false;
                    animator.SetTrigger("Idle");
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // 공격 발사
            Enemy target = EnemyManager.Instance.GetClosestEnemy(transform.position);
            if (target != null)
            {
                float distance = Vector3.Distance(firePoint.position, target.transform.position);
                float arcHeight = Utility.BASE_ARC_HEIGHT + distance * Utility.ARC_HEIGHT_MULTI;

                GameObject arrowObj = Instantiate(prefab_BasicArrowProjectile, firePoint.position, firePoint.rotation);
                arrowObj.GetComponent<ArrowProjectile>().LaunchArc(
                    firePoint.position,
                    target.transform.position,
                    arcHeight,
                    Utility.ARROW_SPEED,
                    stats.Damage,
                    "Enemy"
                );
            }

            // 공격 종료
            isAttacking = false;
            if (Mathf.Abs(moveInput) > 0.01f)
                animator.SetTrigger("Run");
            else
                animator.SetTrigger("Idle");
        }
    }

    private void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
            isAttacking = false;
        }
    }
}
