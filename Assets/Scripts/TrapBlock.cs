using UnityEngine;

public class TrapBlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 충돌한 오브젝트가 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            PlayerState state = other.GetComponent<PlayerState>();

            // [에러 방지] PlayerState 컴포넌트가 없는 경우 처리 중단
            if (state == null) return;

            // 2. 현재 모자 방어막(isInvincible)이 활성화되어 있는지 확인
            if (state.isInvincible)
            {
                // [해결책] 무적 상태라면 방어막 소모 함수를 호출하고 함수 종료
                if (state.Hat != null)
                {
                    state.Hat.ConsumeHatEffect();
                }
                else
                {
                    // Hat 연결이 안 된 경우를 대비한 최소한의 안전 장치
                    state.isInvincible = false;
                    state.UpdateHatUI();
                }

                // *** 중요 ***: 무적일 때는 여기서 return하여 아래의 Respawn()을 실행하지 않음
                return;
            }

            // 3. 무적 상태가 아닐 때만 리스폰 실행
            state.Respawn();

            if (StageManager.Instance != null)
            {
                StageManager.Instance.ShowMessage("Player step on trap!!\nForce move to stage start point!!");
            }
        }
    }
}
