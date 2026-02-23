using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    // 싱글톤 패턴을 적용하여 어디서든 접근 가능하게 합니다.
    public static StageManager Instance;

    public GameObject player;
    [Header("스테이지 설정")]
    public List<GameObject> stages; // Stage1, Stage2 맵 오브젝트들
    public List<Transform> playerSpawnPoints; // 각 스테이지의 시작지점 오브젝트들

    [Header("스테이지 알림 설정")]
    public GameObject stageNoticeUI; // Stage 텍스트가 담긴 UI 오브젝트

    public TMPro.TextMeshProUGUI stageText; // "Stage 1" 등을 표시할 텍스트

    [Header("Pause UI 설정")]
    public GameObject pauseUI;
    public PlayerState playerState;

    public int currentStageLevel = 1;
    private bool isPaused = false;

    // StageManager.cs 또는 UIManager.cs에 추가
    public TextMeshProUGUI noticeText;
    public GameObject noticePanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateStageVisuals();
        noticePanel.SetActive(false);
        isPaused = false;
    }

    public void Update()
    {
        // ESC 키를 누르면 일시정지 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void GoNextStage()
    {
        if (currentStageLevel < stages.Count)
        {
            currentStageLevel++;
            UpdateStageVisuals();
        }
        else
        {
            Debug.Log("마지막 스테이지입니다!");
        }
    }


    private void UpdateStageVisuals()
    {
        // 1. 현재 레벨에 맞는 맵 활성화
        for (int i = 0; i < stages.Count; i++)
        {
            stages[i].SetActive((i + 1) == currentStageLevel);
        }

        // 2. [추가] 현재 스테이지의 매니저를 찾아 스폰 실행
        // 하이라이키 상의 모든 ItemSpawnManager 중 현재 레벨과 일치하는 것을 찾습니다.
        ItemSpawnManager[] spawnManagers = Object.FindObjectsByType<ItemSpawnManager>(FindObjectsSortMode.None);
        foreach (var manager in spawnManagers)
        {
            if (manager.stageLevel == currentStageLevel)
            {
                manager.ExecuteSpawn();
            }
        }

        // 3. [추가] [토네이도 스폰] 현재 레벨의 GameObjectSpawnManager 실행
        GameObjectSpawnManager[] tornadoManagers = Object.FindObjectsByType<GameObjectSpawnManager>(FindObjectsSortMode.None);
        foreach (var tManager in tornadoManagers)
        {
            if (tManager.stageLevel == currentStageLevel)
            {
                tManager.ExecuteTornadoSpawn();
            }
        }

        //4. 플레이어 위치 이동
        if (playerSpawnPoints.Count >= currentStageLevel)
        {
            Vector3 newPos = playerSpawnPoints[currentStageLevel - 1].position;

            // 중요: PlayerState에도 새로운 시작 지점을 알려줘야 함
            if (playerState != null) playerState.currentStartPoint = newPos;

            // 물리 엔진을 사용 중일 경우를 대비해 속도를 0으로 초기화하고 이동
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Unity 6 기준 (구버전은 velocity)
                rb.position = newPos;
            }
            player.transform.position = newPos;
        }
    }

    public void ShowStageNotice()
    {
        if (stageNoticeUI != null)
        {
            stageText.text = "Stage " + (currentStageLevel + 1);
            stageNoticeUI.SetActive(true);
            // 2초 뒤에 자동으로 꺼지게 설정
            Invoke("HideStageNotice", 2.0f);
        }
    }

    private void HideStageNotice()
    {
        stageNoticeUI.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines(); // 이전 메시지 처리 중단
        StartCoroutine(MessageRoutine(message));
    }

    private IEnumerator MessageRoutine(string msg)
    {
        noticeText.text = msg;
        noticePanel.SetActive(true);
        yield return new WaitForSeconds(2.0f); // 2초간 노출
        noticePanel.SetActive(false);
        noticeText.SetText("");
    }


    public void PauseGame()
    {
        isPaused = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0f; // 게임 시간 정지
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1f; // 게임 시간 다시 재생
        UpdateStageVisuals();
        noticePanel.SetActive(false);
    }

    public void Restart()
    {
        // 1. 시간 배속을 정상(1배속)으로 돌려놓습니다.
        Time.timeScale = 1f;

        // 2. [중요] 현재 씬을 다시 로드합니다. 
        // 이 한 줄로 플레이어 위치, 스폰된 아이템, 타이머가 모두 초기화됩니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("모든 오브젝트 위치와 타이머가 초기화되었습니다.");
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        // 메뉴 씬으로 이동 (메뉴 씬이 Build Settings에 등록되어 있어야 함)
        SceneManager.LoadScene("MenuScene");
    }
}