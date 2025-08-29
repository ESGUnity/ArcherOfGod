using UnityEngine;

public static class Utility
{
    // 플레이어 스탯 관련
    public const float PLAYER_BASE_MAX_HEALTH = 1000f;
    public const float PLAYER_BASE_DAMAGE = 50f;
    public const float PLAYER_BASE_ATTACK_SPEED = 1.5f;
    public const float PLAYER_BASE_MOVE_SPEED = 2.3f;

    // 적 스탯 관련
    public const float ENEMY_BASE_MAX_HEALTH = 300f;
    public const float ENEMY_BASE_DAMAGE = 6f;
    public const float ENEMY_BASE_ATTACK_SPEED = 1.5f;
    public const float ENEMY_BASE_MOVE_SPEED = 2.3f;
    // 적 스탯 증가량
    public const float ENEMY_HEALTH_MULTI = 0.2f;
    public const float ENEMY_DAMAGE_MULTI = 0.1f;
    public const float ENEMY_ATTACK_SPEED_MULTI = 0.1f;

    // 스킬 관련
    public const int JUMP_SHOT_ARROW_COUNT = 1;
    public const float JUMP_SHOT_COOLDOWN = 8f;
    public const float JUMP_HEIGHT_Y_VALUE = 3f;
    public const float JUMP_UP_AND_FALL_TIME = 0.3f;
    public const float JUMP_SHOT_ATTACK_SPEED = 0.2f;

    public const int STRAIGHT_SHOT_ARROW_COUNT = 1;
    public const float STRAIGHT_SHOT_COOLDOWN = 6f;
    public const float STRAIGHT_SHOT_ATTACK_SPEED = 0.6f;

    // 적 AI 관련
    public const float ENEMY_MIN_MOVE_DURATION = 1.5f; // 한 방향으로 움직이는 시간
    public const float ENEMY_MAX_MOVE_DURATION = 2.5f; // 한 방향으로 움직이는 시간
    public const int ENEMY_MIN_ARROWS_PER_STOP = 1;
    public const int ENEMY_MAX_ARROWS_PER_STOP = 3;

    // 화살 관련
    public const float ARROW_SPEED = 5.5f; // 화살이 날아가는 속도
    public const float BASE_ARC_HEIGHT = 1.7f; // 화살 궤도의 최소 포물선 높이
    public const float ARC_HEIGHT_MULTI = 0.1f; // 날아가는 거리 증가 시 높이 증가(플레이어와 적 사이의 거리에 따른)

    // UI 관련
    public const float HEALTH_BAR_TWEEN_DURATION = 0.2f;
}
public enum GameStateEnum // 게임 상태 enum
{
    None,
    SetSkill,
    WaitWave,
    Playing,
    GameOver
}
public enum MoveBtnEnum
{
    None, Left, Right
}
public enum SkillBtnEnum
{
    FirstSkill,
    SecondSkill,
    ThirdSkill,
    FourthSkill,
}