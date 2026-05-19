using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private ChaseEnemy[]  enemies;

    public void SpawnFastEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            
        }
    }
}
