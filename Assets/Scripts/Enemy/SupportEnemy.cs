using UnityEngine;
using UnityEngine.AI;

public class SupportEnemy : BaseEnemy
{
    private enum State
    {
        Searching,
        Casting,
        Fleeing
    }

    [Header("Buff")]
    [SerializeField] private float buffRange = 12f;
    [SerializeField] private int maxBuffTargets = 4;

    [SerializeField] private float buffDuration = 6f;
    [SerializeField] private float speedBuff = 1.5f;
    [SerializeField] private float damageBuff = 1.5f;

    [SerializeField] private float castTime = 2f;
    [SerializeField] private float castCooldown = 3f;

    [Header("Fleeing")]
    [SerializeField] private float fleeDistance = 10f;
    [SerializeField] private float fleeSpeed = 5f;

    [SerializeField] private float reactionTime = 0.75f;
    [SerializeField] private float visionHeight = 1.2f;

    [Header("Searching")]
    [SerializeField] private float searchInterval = 0.5f;

    private State currentState;

    private float stateTimer;
    private float searchTimer;

    private Camera playerCamera;

    private void Start()
    {
        playerCamera = Camera.main;

        currentState = State.Searching;
    }

    private void Update()
    {
        if (player == null)
            return;

        switch (currentState)
        {
            case State.Searching:
                UpdateSearching();
                break;

            case State.Casting:
                UpdateCasting();
                break;

            case State.Fleeing:
                UpdateFleeing();
                break;
        }
    }

    private void UpdateSearching()
    {
        navMeshAgent.isStopped = false;

        searchTimer -= Time.deltaTime;

        if (searchTimer > 0f)
            return;

        searchTimer = searchInterval;

        if (PlayerCanSeeMe())
        {
            StartFleeing();
            return;
        }

        if (FindBuffTargets().Count > 0)
        {
            StartCasting();
        }
    }

    private void StartCasting()
    {
        currentState = State.Casting;

        stateTimer = castTime;

        navMeshAgent.isStopped = true;

        FaceNearestTarget();
    }

    private void UpdateCasting()
    {
        navMeshAgent.isStopped = true;

        stateTimer -= Time.deltaTime;

        /*
         * IMPORTANT:
         *
         * We do NOT immediately stop casting when the player
         * sees us.
         *
         * The support has a reaction delay.
         */

        if (stateTimer <= 0f)
        {
            CastBuff();

            stateTimer = castCooldown;

            if (PlayerCanSeeMe())
            {
                // Player caught us immediately after casting.
                StartFleeing();
            }
            else
            {
                currentState = State.Searching;
            }

            return;
        }

        // Player sees us while we're casting.
        if (PlayerCanSeeMe())
        {
            // We continue casting.
            // The player gets a short window to interrupt us.
        }
    }

    private void CastBuff()
    {
        var targets = FindBuffTargets();

        foreach (BaseEnemy enemy in targets)
        {
            enemy.ApplyBuff(
                buffDuration,
                speedBuff,
                damageBuff
            );
        }

        Debug.Log(
            $"{gameObject.name} buffed {targets.Count} enemies."
        );
    }

    private System.Collections.Generic.List<BaseEnemy> FindBuffTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            buffRange
        );

        System.Collections.Generic.List<BaseEnemy> targets =
            new System.Collections.Generic.List<BaseEnemy>();

        foreach (Collider collider in colliders)
        {
            BaseEnemy enemy =
                collider.GetComponentInParent<BaseEnemy>();

            if (enemy == null)
                continue;

            // Don't buff ourselves.
            if (enemy == this)
                continue;

            // Don't buff the same enemy twice.
            if (targets.Contains(enemy))
                continue;

            targets.Add(enemy);

            if (targets.Count >= maxBuffTargets)
                break;
        }

        return targets;
    }

    private void StartFleeing()
    {
        currentState = State.Fleeing;

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = fleeSpeed;

        Vector3 directionAwayFromPlayer =
            transform.position - player.transform.position;

        directionAwayFromPlayer.y = 0f;

        if (directionAwayFromPlayer.sqrMagnitude < 0.01f)
        {
            directionAwayFromPlayer = transform.forward;
        }

        directionAwayFromPlayer.Normalize();

        Vector3 fleeTarget =
            transform.position +
            directionAwayFromPlayer * fleeDistance;

        if (NavMesh.SamplePosition(
            fleeTarget,
            out NavMeshHit hit,
            fleeDistance,
            NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
        else
        {
            // If there isn't a valid point directly behind us,
            // pick a random nearby NavMesh point.
            FindRandomFleePosition();
        }

        stateTimer = reactionTime;
    }

    private void UpdateFleeing()
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.speed = fleeSpeed;

        // Keep running if the player is still nearby.
        if (PlayerCanSeeMe())
        {
            Vector3 directionAwayFromPlayer =
                transform.position - player.transform.position;

            directionAwayFromPlayer.y = 0f;

            if (directionAwayFromPlayer.sqrMagnitude > 0.01f)
            {
                directionAwayFromPlayer.Normalize();

                Vector3 target =
                    transform.position +
                    directionAwayFromPlayer * fleeDistance;

                if (NavMesh.SamplePosition(
                    target,
                    out NavMeshHit hit,
                    fleeDistance,
                    NavMesh.AllAreas))
                {
                    navMeshAgent.SetDestination(hit.position);
                }
            }

            return;
        }

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            navMeshAgent.speed = 3.5f;
            currentState = State.Searching;
        }
    }

    private bool PlayerCanSeeMe()
    {
        if (playerCamera == null)
            return false;

        Vector3 enemyPosition =
            transform.position + Vector3.up * visionHeight;

        Vector3 viewportPosition =
            playerCamera.WorldToViewportPoint(enemyPosition);

        // Enemy isn't inside the camera's view.
        if (viewportPosition.z <= 0f)
            return false;

        if (viewportPosition.x < 0f ||
            viewportPosition.x > 1f ||
            viewportPosition.y < 0f ||
            viewportPosition.y > 1f)
        {
            return false;
        }

        // Check if something is blocking the player's view.
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

    private void FaceNearestTarget()
    {
        var targets = FindBuffTargets();

        if (targets.Count == 0)
            return;

        Vector3 direction =
            targets[0].transform.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }

    private void FindRandomFleePosition()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomDirection =
                Random.insideUnitSphere * fleeDistance;

            randomDirection.y = 0f;

            Vector3 target =
                transform.position + randomDirection;

            if (NavMesh.SamplePosition(
                target,
                out NavMeshHit hit,
                fleeDistance,
                NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            buffRange
        );
    }
}