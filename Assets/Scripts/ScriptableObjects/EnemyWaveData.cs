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
