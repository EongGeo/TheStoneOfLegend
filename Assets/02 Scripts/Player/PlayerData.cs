using System;

[Serializable]
public class PlayerData
{
    public int playerLevel;
    public int playerExp;
    public int playerStr;
    public int playerMaxHp;
    public float playerSpeed;
    public int playerStatPoints;
    public int stageClearNum;

    public static PlayerData GetDefault()
    {
        return new PlayerData
        {
            playerLevel = 1,
            playerExp = 0,
            playerStr = 10,
            playerMaxHp = 10,
            playerSpeed = 3.0f,
            playerStatPoints = 0,
            stageClearNum = 0
        };
    }
}
