using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelUpSystem
{
    public static void LevelUp(ref int level, ref int exp)
    {
        int expRequirement = 100 * level;

        if (exp < expRequirement) return;
        else 
        {
            exp -= expRequirement;
            level++;
            Managers.Game.playerData.playerStatPoints += level;
        }
    }
    public static string ReturnExpPercentage(int level, int exp)
    {
        int expRequirement = 100 * level;
        return $"{exp*100/expRequirement}%";
    }
}
