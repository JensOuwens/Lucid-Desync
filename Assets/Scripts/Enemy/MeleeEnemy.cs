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

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;
        targetUpdateTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        // Close enough to attack.
        if (distanceToPlayer <= attackRange)
        {
            Attack();
            return;
        }

        // Close enough to charge OR player can see us.
        if (distanceToPlayer <= chargeDistance || IsVisibleToPlayer())
        {
            ChargePlayer();
            return;
        }

        // Otherwise, stay behind the player's camera.
        if (targetUpdateTimer <= 0f)
        {
            MoveBehindPlayer();
            targetUpdateTimer = targetUpdateRate;
        }
    }

    private void MoveBehindPlayer()
    {
        Vector3 playerPosition = player.transform.position;

        // We want to be BEHIND where the player is looking.
        Vector3 targetPosition =
            playerPosition - playerCamera.transform.forward * behindPlayerDistance;

        // Don't include camera pitch.
        targetPosition.y = playerPosition.y;

        // Make sure the position is actually on the NavMesh.
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

        // Enemy isn't even inside the camera's view.
        if (viewportPosition.z <= 0)
            return false;

        if (viewportPosition.x < 0 ||
            viewportPosition.x > 1 ||
            viewportPosition.y < 0 ||
            viewportPosition.y > 1)
        {
            return false;
        }

        // Check whether something is blocking the camera's view.
        Vector3 cameraPosition = playerCamera.transform.position;

        Vector3 direction =
            enemyPosition - cameraPosition;

        float distance = direction.magnitude;

        if (Physics.Raycast(
            cameraPosition,
            direction.normalized,
            out RaycastHit hit,
            distance))
        {
            // The first thing the camera hits is the enemy.
            return hit.transform == transform ||
                   hit.transform.IsChildOf(transform);
        }

        return false;
    }

    private void Attack()
    {
        navMeshAgent.isStopped = true;

        Vector3 direction =
            player.transform.position - transform.position;

        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }

        if (attackTimer <= 0f)
        {
            Debug.Log(
                gameObject.name +
                " attacks player for " +
                attackDamage
            );

            player.GetComponent<IDamagable>().Hurt(attackDamage);

            attackTimer = attackCooldown;
        }
    }
}