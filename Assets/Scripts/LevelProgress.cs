using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelProgress
{
    private const string KEY = "UnlockedLevel";
    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(KEY, 1);
    }
    public static void Unlock(int levelIndex)
    {
        int current = GetUnlockedLevel();

        if (levelIndex + 1 > current)
        {
            PlayerPrefs.SetInt(KEY, levelIndex + 1);
            PlayerPrefs.Save();
        }
    }
}
