using System;
using UnityEngine;

public enum EnemyTypes
{
    Melee,
    Projectile,
    Tank,
    Support
}

[System.Serializable]
public struct EnemyWaveData
{
    public EnemyTypes enemyType;
    public int amountOfEnemies;
    public int enemyMaxInLevel;
}

[Serializable]
public struct WaveData
{
    public EnemyWaveData[] enemies;
}
