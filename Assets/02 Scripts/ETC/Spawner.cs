using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    [SerializeField] private Transform stage1SpawnPoints;
    [SerializeField] private Transform stage2SpawnPoints;
    [SerializeField] private Transform stage3SpawnPoints;

    [Header("플레이어/몬스터/보스 프리팹")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject bossPrefab;

    private List<Transform> stageSpawnPoints;

    private void Awake()
    {
        stageSpawnPoints = new List<Transform> { stage1SpawnPoints, stage2SpawnPoints, stage3SpawnPoints };
        Instance = this;
    }
    public void StageSpawn(int stageNum)
    {
        Transform[] spawnPoints = stageSpawnPoints[stageNum-1].GetComponentsInChildren<Transform>();

        Instantiate(playerPrefab, spawnPoints[1].position, Quaternion.identity);
        for (int i = 2; i < spawnPoints.Length-1; i++)
        {
            Instantiate(monsterPrefab, spawnPoints[i].position, Quaternion.identity);
        }
        Instantiate(bossPrefab, spawnPoints[spawnPoints.Length-1].position, Quaternion.identity);

        Managers.Spawn.GetEnemyCount(spawnPoints.Length-2); //루트와 플레이어 제외한 숫자
    }
}
