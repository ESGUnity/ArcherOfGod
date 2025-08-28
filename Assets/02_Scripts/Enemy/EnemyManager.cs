using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // private 필드
    private List<Enemy> enemies = new List<Enemy>();

    // public Getter
    public List<Enemy> Enemies => enemies;

    // 싱글턴
    private static EnemyManager instance;
    public static EnemyManager Instance => instance;

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
    }

    // 메인
    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
    public void UnregisterEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
    public bool HasEnemies()
    {
        return enemies.Count > 0;
    }
    public Enemy GetClosestEnemy(Vector3 position)
    {
        Enemy closest = null;
        float minDist = float.MaxValue;

        foreach (Enemy e in enemies)
        {
            float dist = Vector3.Distance(position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = e;
            }
        }

        return closest;
    }
}
