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

    // private 필드(컴포넌트)
    private Rigidbody2D rb;
    private PlayerStats stats;
    private Coroutine attackRoutine;

    // private 필드
    private float moveInput;

    // 싱글턴
    private static PlayerController instance;
    public static PlayerController Instance => instance;
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

        TryGetComponent(out rb);
        TryGetComponent(out stats);
    }
    private void Update()
    {
#if UNITY_EDITOR
        //moveInput = Input.GetAxisRaw("Horizontal"); // PC 입력
#endif
        // 가만히 있으면 공격 루프 시작, 움직이면 중단
        if (Mathf.Abs(moveInput) < 0.01f)
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
        // 좌우 이동
        rb.linearVelocity = new Vector2(moveInput * stats.MoveSpeed, rb.linearVelocityY);
    }

    // 이동
    public void MoveLeft()
    {
        moveInput = -1f;
    }
    public void MoveRight()
    {
        moveInput = 1f;
    }
    public void StopMove()
    {
        moveInput = 0f;
    }

    // 공격
    private IEnumerator AttackLoop() // moveInput이 0이 된 순간부터 AttackSpeed만큼 기다린 후 발사하는 코루틴
    {
        while (true)
        {
            float wait = Mathf.Max(0.01f, stats.AttackSpeed);
            float elapsed = 0f;

            // 공격속도 동안 공격 준비
            while (elapsed < wait)
            {
                if (Mathf.Abs(moveInput) > 0.01f) // 움직였다면 종료
                {
                    StopAttack(); 
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // 아직도 가만히 있다면 발사
            if (Mathf.Abs(moveInput) < 0.01f)
            {
                ShootArrow();
            }
            else
            {
                StopAttack();
                yield break;
            }
        }
    }
    private void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }
    private void ShootArrow()
    {
        GameObject arrowObj = Instantiate(prefab_BasicArrowProjectile, firePoint.position, firePoint.rotation);
    }
}
