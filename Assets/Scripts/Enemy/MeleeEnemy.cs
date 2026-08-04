using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : BaseEnemy
{
    [Header("Movement")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float chargeDistance = 8f;
    [SerializeField] private float behindPlayerDistance = 6f;
    [SerializeField] private float targetUpdateRate = 0.25f;

    [Header("Combat")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDamage = 10f;

    [Header("Vision")]
    [SerializeField] private float visionRayHeight = 1.2f;

    private float attackTimer;
    private float targetUpdateTimer;

    private Camera playerCamera;
    private float baseMoveSpeed;

    private void Start()
    {
        playerCamera = Camera.main;
        baseMoveSpeed = navMeshAgent.speed;
    }

    private void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;
        targetUpdateTimer -= Time.deltaTime;

        navMeshAgent.speed =
            baseMoveSpeed * GetSpeedMultiplier();

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distanceToPlayer <= attackRange)
        {
            Attack();
            return;
        }

        if (distanceToPlayer <= chargeDistance ||
            IsVisibleToPlayer())
        {
            ChargePlayer();
            return;
        }

        if (targetUpdateTimer <= 0f)
        {
            MoveBehindPlayer();
            targetUpdateTimer = targetUpdateRate;
        }
    }

    private void MoveBehindPlayer()
    {
        Vector3 playerPosition = player.transform.position;

        Vector3 targetPosition =
            playerPosition -
            playerCamera.transform.forward *
            behindPlayerDistance;

        targetPosition.y = playerPosition.y;

        if (NavMesh.SamplePosition(
            targetPosition,
            out NavMeshHit hit,
            behindPlayerDistance,
            NavMesh.AllAreas))
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(hit.position);
        }
    }

    private void ChargePlayer()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.transform.position);
    }

    private bool IsVisibleToPlayer()
    {
        Vector3 enemyPosition = transform.position;
        enemyPosition.y += visionRayHeight;

        Vector3 viewportPosition =
            playerCamera.WorldToViewportPoint(enemyPosition);

        if (viewportPosition.z <= 0)
            return false;

        if (viewportPosition.x < 0 ||
            viewportPosition.x > 1 ||
            viewportPosition.y < 0 ||
            viewportPosition.y > 1)
        {
            return false;
        }

        Vector3 cameraPosition =
            playerCamera.transform.position;

        Vector3 direction =
            enemyPosition - cameraPosition;

        float distance = direction.magnitude;

        if (Physics.Raycast(
            cameraPosition,
            direction.normalized,
            out RaycastHit hit,
            distance))
        {
            return hit.transform == transform ||
                   hit.transform.IsChildOf(transform);
        }

        return false;
    }

    private void Attack()
    {
        navMeshAgent.isStopped = true;

        Vector3 direction =
            player.transform.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }

        if (attackTimer <= 0f)
        {
            float finalDamage =
                attackDamage * GetDamageMultiplier();

            Debug.Log(
                gameObject.name +
                " attacks player for " +
                finalDamage
            );

            if (player.TryGetComponent<IDamagable>(
                out var damageable))
            {
                damageable.Hurt(finalDamage);
            }

            attackTimer = attackCooldown;
        }
    }
}