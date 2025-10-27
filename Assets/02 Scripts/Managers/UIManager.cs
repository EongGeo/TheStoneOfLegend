using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("메인메뉴씬")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject helpPanel;

    [Header("게임씬")]
    [SerializeField] private GameObject playerStatPanel;
    [SerializeField] private GameObject stagePanel;
    [SerializeField] private GameObject saveResultPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private TextMeshProUGUI playerLevelValue;
    [SerializeField] private TextMeshProUGUI expPercentage;
    [SerializeField] private TextMeshProUGUI strValue;
    [SerializeField] private TextMeshProUGUI maxHpValue;
    [SerializeField] private TextMeshProUGUI speedValue;
    [SerializeField] private TextMeshProUGUI statPointsValue;
    [SerializeField] private GameObject stage2Locked;
    [SerializeField] private GameObject stage3Locked;
    [SerializeField] private TextMeshProUGUI saveResultText;

    [Header("스테이지씬")]
    [SerializeField] private GameObject stageResultPanel;
    [SerializeField] private TextMeshProUGUI rewardText;

    private void Start()
    {
        UpdateGameSceneUI();
    }
    private void UpdateGameSceneUI()
    {
        if (playerLevelValue == null || expPercentage == null || strValue == null || maxHpValue == null || speedValue == null || statPointsValue == null)
            return;
        playerLevelValue.text = $"{Managers.Game.playerData.playerLevel}";
        expPercentage.text = LevelUpSystem.ReturnExpPercentage(Managers.Game.playerData.playerLevel, Managers.Game.playerData.playerExp);
        strValue.text = $"{Managers.Game.playerData.playerStr}";
        maxHpValue.text = $"{Managers.Game.playerData.playerMaxHp}";
        speedValue.text = $"{Managers.Game.playerData.playerSpeed}";
        statPointsValue.text = $"{Managers.Game.playerData.playerStatPoints}";
        if(Managers.Game.playerData.stageClearNum > 0) stage2Locked.SetActive(false);
        if(Managers.Game.playerData.stageClearNum > 1) stage3Locked.SetActive(false);
    }
    //게임시작버튼
    public void OnClickGameStartButton()
    {
        SoundManager.Instance.PlayButtonClickSFX();
        Managers.Game.LoadPlayerData();
        SoundManager.Instance.PlayNormalBGM();
        SceneManager.LoadScene("GameScene");
    }
    //설정버튼
    public void OnClickSettingButton()
    {
        SoundManager.Instance.PlayButtonClickSFX();
        if (!settingPanel.activeSelf) settingPanel.SetActive(true);
        else settingPanel.SetActive(false);
    }
    //게임종료버튼
    public void OnClickExitButton()
    {
        SoundManager.Instance.PlayButtonClickSFX();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    //도움말버튼
    public void OnClickHelpButton()
    {
        SoundManager.Instance.PlayButtonClickSFX();
        if (!helpPanel.activeSelf) helpPanel.SetActive(true);
        else helpPanel.SetActive(false);
    }
    //메인메뉴로 돌아가기버튼 + 경고패널버튼
    public void OnClickMainMenuButton()
    {
        SoundManager.Instance.PlayButtonClickSFX();
        if (!mainMenuPanel.activeSelf) mainMenuPanel.SetActive(true);
        else mainMenuPanel.SetActive(false);
    }
    public void OnClickYesButton()
    {
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("MainMenuScene");
    }
    public void OnClickNoButton()
    {
        mainMenuPanel.SetActive(false);
    }

    //플레이어스탯창버튼
    public void OnClickPlayerButton() 
    {
        SoundManager.Instance.PlayButtonClickSFX();
        if (!playerStatPanel.activeSelf) playerStatPanel.SetActive(true);
        else playerStatPanel.SetActive(false);
    }
    //힘증가버튼
    public void OnClickSTRButton()
    {
        SoundManager.Instance.PlayButton2ClickSFX();
        Managers.Game.AddStr();
        UpdateGameSceneUI();
    }
    //최대체력증가버튼
    public void OnClickHPButton()
    {
        SoundManager.Instance.PlayButton2ClickSFX();
        Managers.Game.AddMaxHp();
        UpdateGameSceneUI();
    }
    //이동속도증가버튼
    public void OnClickSPEEDButton()
    {
        SoundManager.Instance.PlayButton2ClickSFX();
        Managers.Game.AddSpeed();
        UpdateGameSceneUI();
    }
    //스테이지선택창버튼
    public void OnClickStageButton()
    {
        SoundManager.Instance.PlayButtonClickSFX();
        if (!stagePanel.activeSelf) stagePanel.SetActive(true);
        else stagePanel.SetActive(false);
    }
    //스테이지1버튼
    public void OnClickStage1Button()
    {
        Managers.Stage.ChooseStage(1);
        SceneManager.LoadScene("StageScene");
    }
    //스테이지2버튼
    public void OnClickStage2Button()
    {
        if(Managers.Game.playerData.stageClearNum < 1) return;
        Managers.Stage.ChooseStage(2);
        SceneManager.LoadScene("StageScene");
    }
    //스테이지3버튼
    public void OnClickStage3Button()
    {
        if (Managers.Game.playerData.stageClearNum < 2) return;
        Managers.Stage.ChooseStage(3);
        SceneManager.LoadScene("StageScene");
    }
    //세이브버튼
    public void OnClickSaveButton()
    {
        bool success = Managers.Game.SavePlayerData();
        StartCoroutine(ShowSaveResult(success));
    }
    //세이브결과
    public IEnumerator ShowSaveResult(bool success)
    {
        saveResultPanel.SetActive(true);

        if (success) saveResultText.text = "Save Completed!";
        else saveResultText.text = "Save Failed!";

        yield return new WaitForSeconds(1);
        saveResultPanel.SetActive(false);
        yield break;
    }
    //다시시작버튼
    public void OnClickRestartButton()
    {
        stageResultPanel.SetActive(false);
        Managers.Stage.Restart();
    }
    //스테이지아웃버튼
    public void OnClickStageOutButton()
    {
        SoundManager.Instance.PlayNormalBGM();
        SceneManager.LoadScene("GameScene");
    }
    //다음스테이지버튼
    public void OnClickNextStageButton()
    {
        stageResultPanel.SetActive(false);
        Managers.Stage.NextStage();
    }
}
