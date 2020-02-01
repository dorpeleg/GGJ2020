using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    private const string ScoreKey = "score";

    public static int GetScore()
    {
        var score = PlayerPrefs.GetInt(ScoreKey, 0);
        return score;
    }

    public static void SaveHighestScore(int score)
    {
        var oldScore = PlayerPrefs.GetInt(ScoreKey, 0);
        if (oldScore >= score) return;
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }
}
