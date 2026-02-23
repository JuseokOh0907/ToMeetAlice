using UnityEngine;
using System.Collections.Generic;

public class ItemSpawnManager : MonoBehaviour
{
    [Header("아이템 프리팹 (순서 중요: 0:안경, 1:모자, 2:회중시계, 3:옷)")]
    public GameObject[] itemPrefabs;
    public Transform[] spawnPoints;

    [Header("스테이지 설정")]
    public int stageLevel = 1;

    // 외부에서 호출할 수 있도록 public 함수로 정의 (OnEnable 제거 권장)
    public void ExecuteSpawn()
    {
        if (spawnPoints.Length == 0 || itemPrefabs.Length < 3) return;

        // 1. 인덱스 셔플 (Fisher-Yates)
        List<int> shuffleIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++) shuffleIndices.Add(i);

        for (int i = shuffleIndices.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = shuffleIndices[i];
            shuffleIndices[i] = shuffleIndices[rand];
            shuffleIndices[rand] = temp;
        }

        // 2. 스테이지별 개수 결정 (호출 시점의 플레이어 상태 체크)
        int glassesCount = 0, clotheCount = 0, hatCount = 0, clockCount = 0;
        PlayerState player = Object.FindFirstObjectByType<PlayerState>();

        if (stageLevel == 1)
        {
            glassesCount = 1;
            hatCount = Random.Range(12, 15);
            clockCount = 25 - glassesCount - hatCount;
        }
        else if (stageLevel == 2)
        {
            clotheCount = 1;
            hatCount = Random.Range(25, 29);
            clockCount = 41 - clotheCount - hatCount;
        }
        else if (stageLevel == 3)
        {
            if (player != null)
            {
                if (!player.isWearingGlasses) glassesCount = 1;
                if (!player.isWearingClothe) clotheCount = 1;
            }
            hatCount = Random.Range(24, 27);
            clockCount = 48 - glassesCount - clotheCount - hatCount;
        }

        // 3. 아이템 배치 실행
        int currentIndex = 0;
        for (int i = 0; i < glassesCount; i++) Spawn(0, shuffleIndices[currentIndex++]);
        for (int i = 0; i < clotheCount; i++) Spawn(3, shuffleIndices[currentIndex++]);
        for (int i = 0; i < hatCount; i++) Spawn(1, shuffleIndices[currentIndex++]);
        for (int i = 0; i < clockCount; i++) Spawn(2, shuffleIndices[currentIndex++]);

        Debug.Log($"[Stage {stageLevel}] 스폰 완료. 안경:{glassesCount}, 옷:{clotheCount}");
    }

    void Spawn(int prefabIndex, int pointIndex)
    {
        if (prefabIndex >= itemPrefabs.Length) return;
        Instantiate(itemPrefabs[prefabIndex], spawnPoints[pointIndex].position, Quaternion.identity);
    }
}