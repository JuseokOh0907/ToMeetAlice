using UnityEngine;
using System.Collections.Generic;

public class GameObjectSpawnManager : MonoBehaviour
{
    [Header("토네이도 설정")]
    [SerializeField] private GameObject tornadoPrefab;

    [Header("스폰 위치 후보들")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("스테이지 설정")]
    public int stageLevel = 1;

    // StageManager에서 호출할 수 있도록 public 함수로 정의합니다.
    public void ExecuteTornadoSpawn()
    {
        if (spawnPoints == null || spawnPoints.Count == 0 || tornadoPrefab == null)
        {
            Debug.LogWarning($"{stageLevel}단계: 스폰 포인트나 프리팹이 설정되지 않았습니다.");
            return;
        }

        // 1. 스테이지별 스폰 개수 결정
        int spawnCount = (stageLevel == 1) ? 2 : 3; // 1단계 2개, 2단계 3개

        // 2. Fisher-Yates 셔플
        List<int> indexList = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++) indexList.Add(i);

        for (int i = indexList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = indexList[i];
            indexList[i] = indexList[randomIndex];
            indexList[randomIndex] = temp;
        }

        // 3. 섞인 순서대로 토네이도 생성
        int actualSpawn = Mathf.Min(spawnCount, indexList.Count);
        for (int i = 0; i < actualSpawn; i++)
        {
            int targetIndex = indexList[i];
            Instantiate(tornadoPrefab, spawnPoints[targetIndex].position, Quaternion.identity);
        }

        Debug.Log($"[Stage {stageLevel}] 토네이도 {actualSpawn}개 스폰 완료.");
    }
}