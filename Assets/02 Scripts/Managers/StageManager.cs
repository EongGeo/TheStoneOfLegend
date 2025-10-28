using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    public int CurStage {  get; private set; }

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
    public void ChooseStage(int stageNum)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayStageBGM();
        CurStage = stageNum;

        float temp = (float)(CurStage - 1) * 50;
        Camera.main.transform.position = new Vector3(temp, 0.0f, -10.0f);

        Managers.Spawn.ConnectSpawner(CurStage);
    }
    public void StageClear()
    {
        UIManager.Instance.GetStageResult(true);
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayStageClearSFX();

        Managers.Game.AddExp(CurStage * 100);
        if (Managers.Game.playerData.stageClearNum < CurStage)
        {
            Managers.Game.playerData.stageClearNum = CurStage;
        }
    }
    public void GameOver()
    {
        UIManager.Instance.GetStageResult(false);
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayGameOverSFX();
    }
    public void Restart()
    {
        SoundManager.Instance.PlayStageBGM();
        ChooseStage(CurStage);
    }
    public void NextStage()
    {
        SoundManager.Instance.PlayStageBGM();
        ChooseStage(CurStage+1);
    }
}
