using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private Transform player;

    [SerializeField] private Transform[] spawnPoints;

    private List<ChaseEnemy> aliveEnemies = new();

    public int EnemiesLeft => aliveEnemies.Count;

    public bool AllEnemiesDead => aliveEnemies.Count == 0;

    public void SpawnFastEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemyObj = Instantiate(
                fastEnemyPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            ChaseEnemy enemy = enemyObj.GetComponent<ChaseEnemy>();
            
            enemy.Initialize(player);

            enemy.IsActive = true;

            enemy.OnDeath += HandleEnemyDeath;

            aliveEnemies.Add(enemy);
        }
    }

    private void HandleEnemyDeath(ChaseEnemy enemy)
    {
        enemy.OnDeath -= HandleEnemyDeath;

        aliveEnemies.Remove(enemy);

        Debug.Log($"Enemies left: {aliveEnemies.Count}");

        if (AllEnemiesDead)
        {
            Debug.Log("All enemies dead");
        }
    }
}