using UnityEngine;
using System;

[Serializable]
public class PlayerInfo
{
    public string playerName;
    public int score;
    public int level;
    public float health;
    public DateTime lastPlayed;

    public PlayerInfo(string name)
    {
        playerName = name;
        score = 0;
        level = 1;
        health = 100f;
        lastPlayed = DateTime.Now;
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
    }

    public void UpdateLevel(int newLevel)
    {
        level = newLevel;
    }

    public void UpdateHealth(float newHealth)
    {
        health = newHealth;
    }

    public void UpdateLastPlayed()
    {
        lastPlayed = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Tên: {playerName}\nĐiểm: {score}\nCấp độ: {level}\nMáu: {health}\nLần chơi cuối: {lastPlayed}";
    }
} 