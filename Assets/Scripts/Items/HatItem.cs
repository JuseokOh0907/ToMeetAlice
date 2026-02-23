using UnityEngine;
using System.Collections;

public class HatItem : ItemAndObject
{
    protected override void OnInteract()
    {
        // 1. 데이터 객체(PlayerState) 가져오기
        PlayerState state = playerTransform.GetComponent<PlayerState>();
        if (state == null) return;

        // 2. 비즈니스 로직 수행 (객체가 스스로 판단)
        if (CanCollect(state))
        {
            Collect(state);
        }
        else
        {
            NotifyMaxLimit();
        }
    }

    // [로직 분리] 획득 가능 여부 확인
    private bool CanCollect(PlayerState state)
    {
        return state.getHatsCount < state.maxHatCount;
    }

    // [로직 분리] 실제 획득 행위
    private void Collect(PlayerState state)
    {
        state.UpdateHatCount(1);
        StageManager.Instance.ShowMessage("Player get hat!!");

        // 획득 시 시각 효과나 사운드가 필요하다면 여기에 추가 (OOP의 장점)
        Destroy(gameObject);
    }

    // [로직 분리] 실패 메시지 알림
    private void NotifyMaxLimit()
    {
        StageManager.Instance.ShowMessage("Hats count already full!!");
    }

    // UI를 갱신하는 전용 함수
    public void UpdateHatUI()
    {
        // playerTransform이 null인 경우를 대비해 FindFirstObjectByType 권장
        PlayerState state = Object.FindFirstObjectByType<PlayerState>();
        if (state != null && state.hatCountText != null)
        {
            state.hatCountText.text = "x " + state.getHatsCount;
        }
    }

    // TrapBlock에서 모자를 소모했을 때도 호출해야 합니다.
    public void ConsumeHatEffect()
    {
        PlayerState state = Object.FindFirstObjectByType<PlayerState>();
        if (state != null && state.isInvincible)
        {
            // 1. 메시지 출력
            if (StageManager.Instance != null)
                StageManager.Instance.ShowMessage("Hat protected by trap!!");

            // 2. [해결책] 파괴되지 않는 PlayerState에서 코루틴 시작 요청
            state.HatCooldown(0.5f);
        }
    }   
}