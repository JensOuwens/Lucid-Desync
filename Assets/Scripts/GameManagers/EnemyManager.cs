using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [Header("Spawn Points")]
    [SerializeField] private Transform[] SpawnPoints;
    [SerializeField] private Transform Player;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject MeleeEnemyPrefab;
    [SerializeField] private GameObject ProjectileEnemyPrefab;
    [SerializeField] private GameObject TankEnemyPrefab;
    [SerializeField] private GameObject SupportEnemyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDistance = 10f;
    [SerializeField] private float spawnInterval = 0.75f;
    [SerializeField] private float spawnPointCooldown = 3f;
    
    private int enemiesAlive;
    private int currentWaveIndex;

    private Coroutine spawnRoutine;

    private List<SpawnGroup> activeGroups = new();

    private Dictionary<Transform, float> spawnPointCooldowns = new();


    // Runtime representation of a WaveGroupData.
    private class SpawnGroup
    {
        public Queue<EnemyTypes> enemies = new();

        public bool IsFinished => enemies.Count == 0;
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void StartLoop(LoopData loopData)
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        currentWaveIndex = 0;
        enemiesAlive = 0;

        spawnRoutine = StartCoroutine(RunLoop(loopData));
    }


    private IEnumerator RunLoop(LoopData loopData)
    {
        foreach (WaveData wave in loopData.enemyData)
        {
            yield return StartCoroutine(RunWave(wave));
        }

        Debug.Log("Loop complete.");
        
        LoopManager.Instance.AllEnemiesDead();
    }


    private IEnumerator RunWave(WaveData wave)
    {
        activeGroups.Clear();

        // Convert all WaveGroupData into runtime SpawnGroups.
        foreach (WaveGroupData groupData in wave.enemies)
        {
            SpawnGroup group = new();

            foreach (EnemyData enemyData in groupData.enemiesInGroup)
            {
                for (int i = 0; i < enemyData.amountOfEnemies; i++)
                {
                    group.enemies.Enqueue(enemyData.enemyType);
                }
            }

            activeGroups.Add(group);
        }

        // All groups are now active at the same time.
        yield return StartCoroutine(SpawnWave(wave));

        // All enemies belonging to this wave have been spawned.
        // Wait until every remaining enemy has died.
        yield return new WaitUntil(() => enemiesAlive <= 0);

        currentWaveIndex++;
    }


    private IEnumerator SpawnWave(WaveData wave)
    {
        while (HasEnemiesToSpawn())
        {
            // Don't exceed the wave's maximum alive enemies.
            if (enemiesAlive >= wave.maxAliveEnemies)
            {
                yield return null;
                continue;
            }

            // Choose which group gets to spawn next.
            SpawnGroup group = GetRandomAvailableGroup();

            if (group == null)
            {
                yield return null;
                continue;
            }

            // Find a suitable spawn point.
            Transform spawnPoint = FindValidSpawnPoint();

            if (spawnPoint == null)
            {
                yield return new WaitForSeconds(0.2f);
                continue;
            }

            // Get the next enemy from the selected group.
            EnemyTypes enemyType = group.enemies.Dequeue();

            SpawnEnemy(enemyType, spawnPoint);

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private bool HasEnemiesToSpawn()
    {
        foreach (SpawnGroup group in activeGroups)
        {
            if (!group.IsFinished)
            {
                return true;
            }
        }

        return false;
    }


    private SpawnGroup GetRandomAvailableGroup()
    {
        List<SpawnGroup> availableGroups = new();

        foreach (SpawnGroup group in activeGroups)
        {
            if (!group.IsFinished)
            {
                availableGroups.Add(group);
            }
        }

        if (availableGroups.Count == 0)
        {
            return null;
        }

        return availableGroups[
            Random.Range(0, availableGroups.Count)
        ];
    }


    private void SpawnEnemy(
        EnemyTypes enemyType,
        Transform spawnPoint)
    {
        GameObject prefab = GetPrefab(enemyType);

        if (prefab == null)
        {
            Debug.LogWarning(
                $"No prefab assigned for enemy type {enemyType}."
            );

            return;
        }

        Instantiate(
            prefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        enemiesAlive++;

        // Prevent this spawn point from being immediately reused.
        spawnPointCooldowns[spawnPoint] = spawnPointCooldown;
    }


    private GameObject GetPrefab(EnemyTypes enemyType)
    {
        return enemyType switch
        {
            EnemyTypes.Melee => MeleeEnemyPrefab,
            EnemyTypes.Projectile => ProjectileEnemyPrefab,
            EnemyTypes.Tank => TankEnemyPrefab,
            EnemyTypes.Support => SupportEnemyPrefab,

            _ => null
        };
    }


    private Transform FindValidSpawnPoint()
    {
        List<Transform> validPoints = new();

        foreach (Transform spawnPoint in SpawnPoints)
        {
            if (IsSpawnPointValid(spawnPoint))
            {
                validPoints.Add(spawnPoint);
            }
        }

        if (validPoints.Count == 0)
        {
            return null;
        }

        return validPoints[
            Random.Range(0, validPoints.Count)
        ];
    }


    private bool IsSpawnPointValid(Transform spawnPoint)
    {
        // Spawn point is on cooldown.
        if (spawnPointCooldowns.TryGetValue(
                spawnPoint,
                out float cooldown))
        {
            if (cooldown > 0f)
            {
                return false;
            }
        }

        // Spawn point is too close to the player.
        if (Vector3.Distance(
                Player.position,
                spawnPoint.position) < minSpawnDistance)
        {
            return false;
        }

        // Player can currently see the spawn point.
        if (CanPlayerSeeSpawnPoint(spawnPoint))
        {
            return false;
        }

        return true;
    }


    private bool CanPlayerSeeSpawnPoint(Transform spawnPoint)
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            return false;
        }

        Vector3 viewportPosition =
            cam.WorldToViewportPoint(spawnPoint.position);

        // Spawn point isn't in front of the camera
        // or isn't currently inside the screen.
        if (viewportPosition.z <= 0f ||
            viewportPosition.x < 0f ||
            viewportPosition.x > 1f ||
            viewportPosition.y < 0f ||
            viewportPosition.y > 1f)
        {
            return false;
        }

        Vector3 direction =
            spawnPoint.position - cam.transform.position;

        if (Physics.Raycast(
            cam.transform.position,
            direction,
            out RaycastHit hit))
        {
            return hit.transform == spawnPoint;
        }

        return false;
    }


    private void Update()
    {
        UpdateSpawnPointCooldowns();
    }


    private void UpdateSpawnPointCooldowns()
    {
        List<Transform> points =
            new(spawnPointCooldowns.Keys);

        foreach (Transform point in points)
        {
            spawnPointCooldowns[point] -= Time.deltaTime;

            if (spawnPointCooldowns[point] <= 0f)
            {
                spawnPointCooldowns.Remove(point);
            }
        }
    }


    public void EnemyDied()
    {
        enemiesAlive = Mathf.Max(
            enemiesAlive - 1,
            0
        );
    }
    
    public void ResetEncounter()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        activeGroups.Clear();

        enemiesAlive = 0;
        currentWaveIndex = 0;

        BaseEnemy[] enemies = FindObjectsByType<BaseEnemy>(
            FindObjectsSortMode.None
        );

        foreach (BaseEnemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }
}