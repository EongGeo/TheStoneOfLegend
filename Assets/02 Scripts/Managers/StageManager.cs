using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    public int ChooseStage(int stageNum)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayStageBGM();
        CurStage = stageNum;
        return stageNum;
    }
    public void StageClear(int stageNum)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayStageClearSFX();
        Managers.Game.AddExp(stageNum*100);
        Managers.Game.playerData.stageClearNum++;
    }
    public void GameOver()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayGameOverSFX();
    }
    public void Restart()
    {
        SoundManager.Instance.PlayStageBGM();
    }
    public void NextStage()
    {
        SoundManager.Instance.PlayStageBGM();
    }
}
