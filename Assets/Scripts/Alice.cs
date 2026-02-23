using UnityEngine;
using UnityEngine.Tilemaps; // 타일맵 데이터 사용을 위해 필수

public class Alice : ItemAndObject
{
    [Header("클리어 구역 설정")]
    [SerializeField] private GameObject clearUI;
    [SerializeField] private TMPro.TextMeshProUGUI resultTimeText;

    protected override void OnInteract()
    {
        // 1. 이제 타일맵 체크 없이, 상호작용 범위 안이기만 하면 실행됩니다.
        PlayerState state = playerTransform.GetComponent<PlayerState>();

        if (state != null)
        {
            // 2. 옷을 입었는지 확인
            if (state.isWearingClothe)
            {
                GameClear();
            }
            else
            {
                if (StageManager.Instance != null)
                    StageManager.Instance.ShowMessage("Oops! I need to wear clothes to meet Alice!");
            }
        }
    }

    private void GameClear()
    {
        if (clearUI != null)
        {
            clearUI.SetActive(true);
            SoundManager.Instance.PlaySFX(SoundManager.Instance.clearSound);

            // 3. 타이머 시간 표시 (Congratulations!!와 함께)
            if (resultTimeText != null && StageManager.Instance != null)
            {
                resultTimeText.text = "Congratulations!!\nClear Time: " + GameTimer.Instance.timerText.text;
            }
        }

        Time.timeScale = 0f; // 게임 정지
        Debug.Log("★ 게임 클리어 ★");
    }
}
