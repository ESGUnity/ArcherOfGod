using UnityEngine;

public static class Consts
{
    // 플레이어 스탯 관련
    public const float PLAYER_BASE_MAX_HEALTH = 1000f;
    public const float PLAYER_BASE_DAMAGE = 10f;
    public const float PLAYER_BASE_ATTACK_SPEED = 1.5f;
    public const float PLAYER_BASE_MOVE_SPEED = 3f;

    // 적 스탯 관련
    public const float ENEMY_BASE_MAX_HEALTH = 500f;
    public const float ENEMY_BASE_DAMAGE = 6f;
    public const float ENEMY_BASE_ATTACK_SPEED = 1.5f;
    public const float ENEMY_BASE_MOVE_SPEED = 3f;

    // 화살 관련
    public const float ARROW_SPEED = 10f; // 화살이 날아가는 속도
    public const float BASE_ARC_HEIGHT = 2f; // 화살 궤도의 최소 포물선 높이
    public const float ARC_HEIGHT_MULTI = 0.2f; // 날아가는 거리 증가 시 높이 증가(플레이어와 적 사이의 거리에 따른)

    // UI 관련
    public const float HEALTH_BAR_TWEEN_DURATION = 0.2f;
}
