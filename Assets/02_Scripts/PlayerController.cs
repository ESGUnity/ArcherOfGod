using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // private 필드(컴포넌트)
    private Rigidbody2D rb;
    private PlayerStats stats;

    // private 필드
    private float moveInput;

    // 싱글턴
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }
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
        // PC 입력
        //moveInput = Input.GetAxisRaw("Horizontal");
#endif
    }
    private void FixedUpdate()
    {
        // 좌우 이동
        rb.linearVelocity = new Vector2(moveInput * stats.MoveSpeed, rb.linearVelocityY);
    }

    // 메인
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
}
