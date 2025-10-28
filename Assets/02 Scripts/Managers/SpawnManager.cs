using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public int EnemyCount {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GetEnemyCount(int enemyCount)
    {
        EnemyCount = enemyCount;
    }
    public void EnemyDie()
    {
        EnemyCount--;
        if (EnemyCount == 0)
        {
            Managers.Stage.StageClear();
        }
    }
    public void ConnectSpawner(int stageNum)
    {
        StartCoroutine(WakeUp(stageNum));
    }
    IEnumerator WakeUp(int stageNum)
    {
        yield return null; //Spawner 일어날 시간 벌기 -> Spawner의 Awake보다 메서드 실행이 빨라 null참조오류가 발생하는 것으로 추정하여 추가
        Spawner.Instance.StageSpawn(stageNum);
    }
}
