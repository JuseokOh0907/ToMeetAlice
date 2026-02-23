using UnityEngine;

public class ClockItem : ItemAndObject
{
    public float stopTimeAmount = 60f;

    protected override void OnInteract()
    {
        // 싱글톤 인스턴스를 바로 사용합니다.
        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.StopTimer(stopTimeAmount);

            if (StageManager.Instance != null)
            {
                StageManager.Instance.ShowMessage($"Time Stopped for {stopTimeAmount}s!");
            }

            Destroy(gameObject);
        }
    }
}