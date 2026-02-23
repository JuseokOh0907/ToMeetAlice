using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필수

public class MainMenu : MonoBehaviour
{
    // Start 버튼 연결용 함수
    public void StartGame()
    {
        // "InGameStage" 부분에 실제 게임 플레이 씬의 이름을 입력하세요
        SceneManager.LoadScene("GameScene");
    }

    // Exit 버튼 연결용 함수
    public void QuitGame()
    {
        Debug.Log("게임 종료!");
        Application.Quit(); // 실제 빌드된 게임에서만 작동합니다
    }
}