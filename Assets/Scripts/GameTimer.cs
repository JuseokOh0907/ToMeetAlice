using UnityEngine;
using TMPro;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;
    public float timeLimit = 1800f;
    private bool isPaused = false;
    public TextMeshProUGUI timerText;

    [Header("정지 아이템 설정")]
    private float stopRemainingTime = 0f; // 남은 정지 시간
    private Color originalColor = Color.white;
    public Color stopColor = Color.red;

    [Header("UI 연결")]
    public GameObject clearUI;
    public TextMeshProUGUI resultTimeText;

    private void Awake()
    {
        Instance = this;
        if (timerText != null) originalColor = timerText.color;
    }

    void Update()
    {
        // 1. 아이템으로 인한 정지 시간 체크
        if (stopRemainingTime > 0)
        {
            stopRemainingTime -= Time.deltaTime;
            UpdateStopEffects(); // 색상 및 깜빡임 업데이트
            return; // 정지 중에는 아래의 timeLimit 감소를 실행하지 않음
        }
        else if (isPaused) // stopRemainingTime이 끝난 직후 처리
        {
            ResumeTimer();
        }

        // 2. 일반 타이머 흐름
        if (timeLimit > 0)
        {
            timeLimit -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeLimit = 0;
            Debug.Log("Game Over!");
        }
    }

    // 아이템 효과 적용 (두 번째 방식: 시간 초기화)
    public void StopTimer(float duration)
    {
        isPaused = true;
        stopRemainingTime = duration; // 기존 시간이 얼마든 duration으로 초기화 (Reset)
        timerText.color = stopColor;
    }

    private void UpdateStopEffects()
    {
        // 남은 시간이 3초 미만일 때 깜빡임 연출
        if (stopRemainingTime < 10.0f)
        {
            float alpha = Mathf.PingPong(Time.time * 10f, 1f); // 빠르게 깜빡임
            timerText.alpha = alpha;
        }
        else
        {
            timerText.alpha = 1.0f;
        }
    }

    private void ResumeTimer()
    {
        isPaused = false;
        stopRemainingTime = 0f;
        timerText.color = originalColor;
        timerText.alpha = 1.0f;
    }

    // 기존 함수들 유지
    public void AddTime(float amount) => timeLimit += amount;

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeLimit / 60);
            int seconds = Mathf.FloorToInt(timeLimit % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void ShowClearWindow()
    {
        clearUI.SetActive(true);
        // 고정된 Find 대신 Instance 사용 권장
        resultTimeText.text = "Clear Time: " + timerText.text;
        Time.timeScale = 0f;
    }
}