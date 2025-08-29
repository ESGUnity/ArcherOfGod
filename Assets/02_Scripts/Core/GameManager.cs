using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_Player;
    [SerializeField] private GameObject prefab_Enemy;

    // 씬 오브젝트
    [Header("씬 오브젝트")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private GameObject text_WaveAlarm;
    [SerializeField] private GameObject text_RewardAlarm;

    // private 필드
    private GameStateEnum currentState; // 현재 게임 상태
    private int currentWave = 0; // 현재 웨이브 단계
    private Vector3 waveTextOriginPos = new Vector3(0, 100, 0);
    private string rewardAlarmText;
    private Vector3 rewardTextOriginPos = new Vector3(0, -50, 0);

    // public Getter
    public GameStateEnum CurrentState => currentState;
    public int CurrentWave => currentWave;

    // 싱글턴
    private static GameManager instance;
    public static GameManager Instance => instance;

    // 유니티 콜백
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
    }
    private void Start()
    {
        ChangeState(GameStateEnum.SetSkill);
        StartCoroutine(GameFlowRoutine());
    }

    // 메인
    private IEnumerator GameFlowRoutine() // 게임의 전체 흐름 코루틴
    {
        yield return new WaitForSeconds(1f); // 1초 대기

        // 웨이브 루프 시작
        while (true)
        {
            currentWave++; // 웨이브 증가
            ChangeState(GameStateEnum.WaitWave);

            // 적 스폰
            SpawnEnemies();
            // 플레이어 버프
            BuffPlayer();
            // Wave 텍스트 띄우기
            yield return StartCoroutine(ShowWaveText(currentWave)); // 대기

            // 전투 시작
            ChangeState(GameStateEnum.Playing);

            // 적 전멸 대기
            while (EnemyManager.Instance.HasEnemies())
            {
                yield return null;
            }

            // 다음 웨이브 준비 대기
            ChangeState(GameStateEnum.WaitWave);
            yield return new WaitForSeconds(2f);
        }
    }

    // 유틸
    public void ChangeState(GameStateEnum newState)
    {
        currentState = newState;
        Debug.Log($"현재 상태 : {currentState} 시작");

    }
    private void SpawnEnemies()
    {
        int count = Random.Range(1, 4); // 1에서 3마리

        List<Transform> tempPos = enemySpawnPoints.ToList();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, tempPos.Count);

            GameObject go = Instantiate(prefab_Enemy);
            go.transform.position = tempPos[randomIndex].position;
            tempPos.RemoveAt(randomIndex);
        }
    }
    private IEnumerator ShowWaveText(int wave)
    {
        // Wave 텍스트
        text_WaveAlarm.gameObject.SetActive(true);
        var tmpWave = text_WaveAlarm.GetComponent<TMP_Text>();
        tmpWave.text = $"Wave {wave}";

        RectTransform rectWave = text_WaveAlarm.GetComponent<RectTransform>();
        rectWave.anchoredPosition = waveTextOriginPos;
        tmpWave.alpha = 0f;

        // Reward 텍스트
        text_RewardAlarm.gameObject.SetActive(true);
        var tmpReward = text_RewardAlarm.GetComponent<TMP_Text>();
        tmpReward.text = rewardAlarmText; // 필드에 미리 할당된 문자열 사용

        RectTransform rectReward = text_RewardAlarm.GetComponent<RectTransform>();
        rectReward.anchoredPosition = waveTextOriginPos + new Vector3(0, -150f, 0); // Wave 텍스트 아래쪽
        tmpReward.alpha = 0f;

        // DOTween Sequence
        Sequence seq = DOTween.Sequence();

        // Wave 텍스트 애니메이션
        seq.Append(tmpWave.DOFade(1f, 0.5f));
        seq.Join(rectWave.DOAnchorPosY(waveTextOriginPos.y + 50f, 0.5f));

        // Reward 텍스트 애니메이션
        seq.Join(tmpReward.DOFade(1f, 0.5f));
        seq.Join(rectReward.DOAnchorPosY(rectReward.anchoredPosition.y + 50f, 0.5f));

        seq.AppendInterval(2f); // 대기

        // Fade out
        seq.Append(tmpWave.DOFade(0f, 0.5f));
        seq.Join(rectWave.DOAnchorPosY(waveTextOriginPos.y, 0.5f));
        seq.Join(tmpReward.DOFade(0f, 0.5f));
        seq.Join(rectReward.DOAnchorPosY(rectReward.anchoredPosition.y - 50f, 0.5f));

        seq.OnComplete(() =>
        {
            text_WaveAlarm.SetActive(false);
            text_RewardAlarm.SetActive(false);
        });

        yield return seq.WaitForCompletion();
    }
    private void BuffPlayer()
    {
        // enum에서 무작위 하나 선택
        PlayerBuffEnum randomBuff = (PlayerBuffEnum)Random.Range(0, System.Enum.GetValues(typeof(PlayerBuffEnum)).Length);

        switch (randomBuff)
        {
            case PlayerBuffEnum.AttackSpeedUp:
                PlayerController.Instance.GetComponent<PlayerStats>().ModifyAttackSpeed(-0.1f);
                rewardAlarmText = "AttackSpeedUp!";
                break;

            case PlayerBuffEnum.DamageUp:
                PlayerController.Instance.GetComponent<PlayerStats>().ModifyDamage(5);
                rewardAlarmText = "DamageUp!";
                break;

            case PlayerBuffEnum.MoveSpeedUp:
                PlayerController.Instance.GetComponent<PlayerStats>().ModifyMoveSpeed(0.2f); 
                rewardAlarmText = "MoveSpeedUp!";
                break;

            case PlayerBuffEnum.JumpShotArrowCountUp:
                PlayerController.Instance.GetComponent<PlayerStats>().ModifyJumpShotArrowCount(1);
                rewardAlarmText = "JumpShotArrowCountUp!";
                break;

            case PlayerBuffEnum.MultiShotArrowCountUp:
                PlayerController.Instance.GetComponent<PlayerStats>().ModifyMultiShotArrowCount(1);
                rewardAlarmText = "MultiShotArrowCountUp!";
                break;

            default:
                Debug.LogWarning("Unknown buff type!");
                rewardAlarmText = "Unknown Buff!";
                break;
        }
    }



    // 게임 플로우 제어
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("게임 일시정지");
    }
    public void ResumeGame()
    {
        ChangeState(GameStateEnum.Playing);
        Time.timeScale = 1f;
        Debug.Log("게임 재개");
    }
    public void EndGame()
    {
        ChangeState(GameStateEnum.GameOver);
        Debug.Log("게임 종료");
    }
}
