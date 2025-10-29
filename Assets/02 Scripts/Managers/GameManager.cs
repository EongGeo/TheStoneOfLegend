using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public PlayerData playerData;
    public bool SaveSuccess {  get; set; }
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
    public bool SavePlayerData()
    {
        SaveSystem.Save(playerData);
        return SaveSuccess;
    }
    public void LoadPlayerData()
    {
        if (SaveSystem.TryLoad(out PlayerData loaded))
        {
            playerData = loaded;
            Debug.Log("플레이어 데이터 로드 성공");
        }
        else 
        {
            playerData = loaded;
            Debug.Log("디폴트 로드 성공");
        }
    }
    public void AddExp(int rewardExp)
    {
        playerData.playerExp += rewardExp;
        LevelUpSystem.LevelUp(ref playerData.playerLevel, ref playerData.playerExp);
    }
    public void AddStr()
    {
        if (playerData.playerStatPoints <= 0) return;

        playerData.playerStr++;
        playerData.playerStatPoints--;      
    }
    public void AddMaxHp()
    {
        if (playerData.playerStatPoints <= 0) return;

        playerData.playerMaxHp += 2;
        playerData.playerStatPoints--;
    }
    public void AddSpeed()
    {
        if (playerData.playerStatPoints <= 0) return;

        float temp = playerData.playerSpeed + 0.1f;
        temp = Mathf.Round(temp * 10.0f) / 10.0f; //뒤에 소수부분이 9999999 나오는 현상 해결을 위한 반올림
        playerData.playerSpeed = temp;
        playerData.playerStatPoints--;
    }
}
