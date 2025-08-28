using UnityEngine;

public class Enemy : MonoBehaviour
{
    // private 필드(컴포넌트)
    private EnemyStats stats;
    private EnemyAI aI;

    // public Getter
    public EnemyStats Stats => stats;
    public EnemyAI AI => aI;

    // 유니티 콜백
    private void Awake()
    {
        TryGetComponent(out stats);
        TryGetComponent(out aI);
    }
    private void OnEnable()
    {
        EnemyManager.Instance.RegisterEnemy(this);
    }
    private void OnDisable()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
    }
}
