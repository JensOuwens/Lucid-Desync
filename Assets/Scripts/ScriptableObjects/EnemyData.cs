using System;
using UnityEngine;

public enum EnemyTypes
{
    Melee,
    Projectile,
    Tank,
    Support
}

[Serializable]
public struct WaveData
{
    public WaveGroupData[] enemies;
    public int maxAliveEnemies;
}

[Serializable]
public struct WaveGroupData
{
    public EnemyData[] enemiesInGroup;
}

[Serializable]
public struct EnemyData
{
    public EnemyTypes enemyType;
    public int amountOfEnemies;
}
