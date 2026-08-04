using UnityEngine;
using UnityEngine.AI;

public class ProjectileEnemy : BaseEnemy
{
    private enum State
    {
        Moving,
        Attacking,
        Repositioning
    }

    [Header("Combat")]
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float damage = 10f;

    [Header("Repositioning")]
    [SerializeField] private float repositionDistance = 4f;
    [SerializeField] private float repositionTime = 0.75f;

    [Header("Vision")]
    [SerializeField] private float visionHeight = 1.2f;
    [SerializeField] private float projectileAimHeight = 0.6f;

    private State currentState;

    private float attackTimer;
    private float repositionTimer;

    private float baseMoveSpeed;

    private void Start()
    {
        baseMoveSpeed = navMeshAgent.speed;
        currentState = State.Moving;
    }

    private void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;

        navMeshAgent.speed =
            baseMoveSpeed * GetSpeedMultiplier();

        switch (currentState)
        {
            case State.Moving:
                UpdateMoving();
                break;

            case State.Attacking:
                UpdateAttacking();
                break;

            case State.Repositioning:
                UpdateRepositioning();
                break;
        }
    }

    private void UpdateMoving()
    {
        navMeshAgent.isStopped = false;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance > attackRange)
        {
            navMeshAgent.SetDestination(
                player.transform.position
            );

            return;
        }

        if (HasLineOfSight())
        {
            navMeshAgent.isStopped = true;
            currentState = State.Attacking;
            return;
        }

        navMeshAgent.SetDestination(
            player.transform.position
        );
    }

    private void UpdateAttacking()
    {
        navMeshAgent.isStopped = true;

        FacePlayer();

        if (!HasLineOfSight())
        {
            currentState = State.Moving;
            return;
        }

        if (attackTimer <= 0f)
        {
            Shoot();

            attackTimer = attackCooldown;

            StartRepositioning();
        }
    }

    private void StartRepositioning()
    {
        Vector3 directionFromPlayer =
            transform.position -
            player.transform.position;

        directionFromPlayer.y = 0f;

        if (directionFromPlayer.sqrMagnitude < 0.01f)
        {
            directionFromPlayer = transform.right;
        }

        directionFromPlayer.Normalize();

        Vector3 repositionDirection =
            Vector3.Cross(
                Vector3.up,
                directionFromPlayer
            );

        if (Random.value > 0.5f)
        {
            repositionDirection *= -1f;
        }

        Vector3 targetPosition =
            transform.position +
            repositionDirection *
            repositionDistance;

        if (NavMesh.SamplePosition(
            targetPosition,
            out NavMeshHit hit,
            repositionDistance,
            NavMesh.AllAreas))
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(hit.position);

            repositionTimer = repositionTime;
            currentState = State.Repositioning;
        }
        else
        {
            currentState = State.Attacking;
        }
    }

    private void UpdateRepositioning()
    {
        navMeshAgent.isStopped = false;

        repositionTimer -= Time.deltaTime;

        if (repositionTimer <= 0f)
        {
            if (HasLineOfSight())
            {
                navMeshAgent.isStopped = true;
                currentState = State.Attacking;
            }
            else
            {
                currentState = State.Moving;
            }
        }
    }

    private void Shoot()
    {
        FacePlayer();

        EnemyProjectile projectile =
            EnemyProjectilePool.Instance.Get();

        Vector3 target =
            player.transform.position +
            Vector3.up * projectileAimHeight;

        Vector3 direction =
            target -
            projectileSpawnPoint.position;

        projectile.transform.SetPositionAndRotation(
            projectileSpawnPoint.position,
            Quaternion.LookRotation(direction)
        );

        float finalDamage =
            damage * GetDamageMultiplier();

        projectile.Initialize(finalDamage);
    }

    private bool HasLineOfSight()
    {
        Vector3 origin =
            transform.position +
            Vector3.up * visionHeight;

        Vector3 target =
            player.transform.position +
            Vector3.up * visionHeight;

        Vector3 direction = target - origin;

        if (Physics.Raycast(
            origin,
            direction.normalized,
            out RaycastHit hit,
            direction.magnitude))
        {
            return hit.transform == player.transform ||
                   hit.transform.IsChildOf(player.transform);
        }

        return false;
    }

    private void FacePlayer()
    {
        Vector3 direction =
            player.transform.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }
}