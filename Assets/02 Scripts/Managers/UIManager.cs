using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TextMeshProUGUI stage2Locked;
    [SerializeField] private TextMeshProUGUI stage3Locked;
    [SerializeField] private TextMeshProUGUI saveResultText;

    [Header("스테이지씬")]
    [SerializeField] private GameObject stageResultPanel;
    [SerializeField] private TextMeshProUGUI rewardText;

    //게임시작버튼
    public void OnClickGameStartButton()
    {
        SoundManager.Instance.PlayButtonClickSFX();
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
    //메인메뉴로 돌아가기버튼
    public void OnClickMainMenuButton()
    {
        SoundManager.Instance.PlayButtonClickSFX();
        if (!mainMenuPanel.activeSelf) mainMenuPanel.SetActive(true);
        else mainMenuPanel.SetActive(false);
    }
    public void OnClickYesButton()
    {
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

    }
    //최대체력증가버튼
    public void OnClickHPButton()
    {

    }
    //이동속도증가버튼
    public void OnClickSPEEDButton()
    {

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
        SceneManager.LoadScene("StageScene");
    }
    //스테이지2버튼
    public void OnClickStage2Button()
    {

    }
    //스테이지3버튼
    public void OnClickStage3Button()
    {

    }
    //세이브버튼
    public void OnClickSaveButton()
    {

    }
    //다시시작버튼
    public void OnClickRestartButton()
    {

    }
    //스테이지아웃버튼
    public void OnClickStageOutButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    //다음스테이지버튼
    public void OnClickNextStageButton()
    {

    }
}
