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

    // private 필드
    private GameStateEnum currentState; // 현재 게임 상태
    private int currentWave = 0; // 현재 웨이브 단계
    private Vector3 waveTextOriginPos = new Vector3(0, 100, 0);

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
        // 스킬 선택 루틴 (임시)
        // TODO : 나중에는 선택 확인이 올 때까지 대기하는 루틴으로 변경
        ChangeState(GameStateEnum.SetSkill);
        Debug.Log("스킬 선택 : 5개를 고르세요...");
        yield return new WaitForSeconds(2f); // 선택 대기 시뮬레이션

        // 웨이브 루프 시작
        while (true)
        {
            currentWave++; // 웨이브 증가
            Debug.Log($"Wave {currentWave} 시작!");
            ChangeState(GameStateEnum.WaitWave);

            // 적 스폰
            SpawnEnemies();
            // Wave 텍스트 띄우기
            yield return StartCoroutine(ShowWaveText(currentWave));

            // 전투 시작
            ChangeState(GameStateEnum.Playing);
            Debug.Log("전투 시작!");

            // 적 전멸 대기
            while (EnemyManager.Instance.HasEnemies())
            {
                yield return null; // 한 프레임 대기
            }
            Debug.Log("적 전멸!");

            // 다음 웨이브 준비 대기
            ChangeState(GameStateEnum.WaitWave);
            Debug.Log("다음 웨이브 준비 (2초 대기)");
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

            Instantiate(prefab_Enemy, tempPos[randomIndex].position, Quaternion.identity);
            tempPos.RemoveAt(randomIndex);
        }
    }
    private IEnumerator ShowWaveText(int wave)
    {
        text_WaveAlarm.gameObject.SetActive(true);
        var tmp = text_WaveAlarm.GetComponent<TMP_Text>();
        tmp.text = $"Wave {wave}";

        // RectTransform 가져오기
        RectTransform rect = text_WaveAlarm.GetComponent<RectTransform>();

        // 초기 세팅
        rect.anchoredPosition = waveTextOriginPos; // RectTransform 기준
        tmp.alpha = 0f;

        // DOTween Sequence 생성
        Sequence seq = DOTween.Sequence();
        seq.Append(tmp.DOFade(1f, 0.5f));
        seq.Join(rect.DOAnchorPosY(waveTextOriginPos.y + 50f, 0.5f)); // 2f 대신 100f 정도로 UI 단위
        seq.AppendInterval(2f); // 2초 대기
        seq.Append(tmp.DOFade(0f, 0.5f));
        seq.Join(rect.DOAnchorPosY(waveTextOriginPos.y, 0.5f));
        seq.OnComplete(() => text_WaveAlarm.SetActive(false));

        yield return seq.WaitForCompletion();
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
