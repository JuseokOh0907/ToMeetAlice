using UnityEngine;
using System.Collections;

public class TornadoItem : ItemAndObject
{
    [Header("토네이도 애니메이션 연결")]
    [SerializeField] private Animator Tornado;
    [SerializeField] private float transitionDelay = 1.0f; 
    private bool isTransitioning = false;

    protected override void OnInteract()
    {
        if (isTransitioning) return;
        StartCoroutine(StageTransition());
    }

    private IEnumerator StageTransition()
    {
        isTransitioning = true;

        if (Tornado != null)
        {
            // 1. 애니메이션 재생 (대기 상태 -> Tornado_In)
            Tornado.SetTrigger("OnCollect");
        }

        // 2. 스테이지 전환 알림 시스템 호출 (아래 2번에서 구현)
        if (StageManager.Instance != null)
        {
            StageManager.Instance.ShowStageNotice();
        }

        yield return new WaitForSeconds(transitionDelay);

        // 3. 실제 스테이지 이동 (이 시점엔 화면이 가려져 있음)
        if (StageManager.Instance != null)
        {
            StageManager.Instance.GoNextStage();
        }

        Destroy(gameObject);
    }
}