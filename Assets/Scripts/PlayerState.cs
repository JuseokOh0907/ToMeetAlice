using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    [Header("Item Status")]
    public bool isWearingGlasses = false;
    public bool isWearingClothe = false;
    public bool isInvincible = false;

    [Header("Hat Settings")]
    public int getHatsCount = 0;
    public readonly int maxHatCount = 10;
    public TextMeshProUGUI hatCountText;

    [Header("Player Movement")]
    public GameObject Player;
    [HideInInspector] public Vector3 currentStartPoint;

    public HatItem Hat;

    void Start()
    {
        Hat.UpdateHatUI(); // 시작할 때 초기화
    }

    // 모자 개수 변경 및 UI 갱신
    public void UpdateHatCount(int amount)
    {
        getHatsCount = Mathf.Clamp(getHatsCount + amount, 0, maxHatCount);
        UpdateHatUI();
    }

    public void UpdateHatUI()
    {
        if (hatCountText != null) hatCountText.text = "x " + getHatsCount;
    }

    // 함정 충돌 시 리스폰 로직
    public void Respawn()
    {
        Player.transform.position = currentStartPoint;
    }

    public void HatCooldown(float duration)
    {
        // 플레이어는 함정에 닿아도 파괴되지 않으므로 코루틴이 끝까지 실행됩니다.
        StartCoroutine(HatRoutine(duration));
    }

    private IEnumerator HatRoutine(float duration)
    {
        yield return new WaitForSeconds(duration); // 0.5초 대기

        isInvincible = false;
        UpdateHatUI();
        Debug.Log("방어막이 정상적으로 해제되었습니다.");
    }
}