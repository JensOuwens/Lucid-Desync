using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilePool : MonoBehaviour
{
    public static EnemyProjectilePool Instance { get; private set; }

    [SerializeField] private EnemyProjectile projectilePrefab;
    [SerializeField] private int initialPoolSize = 30;

    private readonly Queue<EnemyProjectile> pool = new();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < initialPoolSize; i++)
        {
            EnemyProjectile projectile =
                Instantiate(projectilePrefab, transform);

            projectile.gameObject.SetActive(false);
            pool.Enqueue(projectile);
        }
    }

    public EnemyProjectile Get()
    {
        EnemyProjectile projectile;

        if (pool.Count > 0)
        {
            projectile = pool.Dequeue();
        }
        else
        {
            projectile =
                Instantiate(projectilePrefab, transform);
        }

        projectile.gameObject.SetActive(true);

        return projectile;
    }

    public void Return(EnemyProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
        pool.Enqueue(projectile);
    }
}