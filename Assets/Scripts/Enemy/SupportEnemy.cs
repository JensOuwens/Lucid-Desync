using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SupportEnemy : BaseEnemy
{
    private enum State
    {
        Searching,
        Casting,
        Reacting,
        Fleeing
    }

    [Header("Buff")]
    [SerializeField] private float buffRange = 12f;
    [SerializeField] private int maxBuffTargets = 4;

    [SerializeField] private float buffDuration = 6f;
    [SerializeField] private float speedBuff = 1.5f;
    [SerializeField] private float damageBuff = 1.5f;

    [Header("Casting")]
    [SerializeField] private float castTime = 2f;
    [SerializeField] private float castCooldown = 3f;

    [Header("Fleeing")]
    [SerializeField] private float fleeDistance = 15f;
    [SerializeField] private float fleeSpeed = 5f;

    [SerializeField] private int fleeSamples = 20;

    [Header("Vision")]
    [SerializeField] private float visionHeight = 0.25f;

    [Header("Searching")]
    [SerializeField] private float searchInterval = 0.5f;

    private State currentState;

    private float stateTimer;
    private float searchTimer;

    private Camera playerCamera;

    private float baseMoveSpeed;

    private bool playerSpottedDuringCast;

    private void Start()
    {
        playerCamera = Camera.main;

        baseMoveSpeed = navMeshAgent.speed;

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

            case State.Reacting:
                UpdateReacting();
                break;

            case State.Fleeing:
                UpdateFleeing();
                break;
        }
    }

    private void UpdateSearching()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed =
            baseMoveSpeed * GetSpeedMultiplier();

        searchTimer -= Time.deltaTime;

        if (searchTimer > 0f)
            return;

        searchTimer = searchInterval;

        // Player spotted us.
        if (CanSeePlayer())
        {
            StartReacting();
            return;
        }

        // Look for enemies to buff.
        if (FindBuffTargets().Count > 0)
        {
            StartCasting();
        }
    }

    private void StartCasting()
    {
        currentState = State.Casting;

        stateTimer = castTime;
        playerSpottedDuringCast = false;

        navMeshAgent.isStopped = true;

        FaceNearestTarget();
    }

    private void UpdateCasting()
    {
        navMeshAgent.isStopped = true;

        stateTimer -= Time.deltaTime;

        // Remember that the player saw us.
        if (CanSeePlayer())
        {
            playerSpottedDuringCast = true;
        }

        // Finish casting.
        if (stateTimer <= 0f)
        {
            CastBuff();

            // We were spotted during the cast.
            if (playerSpottedDuringCast || CanSeePlayer())
            {
                StartReacting();
            }
            else
            {
                stateTimer = castCooldown;
                currentState = State.Searching;
            }
        }
    }

    private void StartReacting()
    {
        /*
         * IMPORTANT:
         * We leave Casting immediately.
         *
         * This means the support cannot finish another buff
         * once it has decided to flee.
         */
        currentState = State.Reacting;

        stateTimer = 0.75f;

        navMeshAgent.isStopped = true;

        FacePlayer();
    }

    private void UpdateReacting()
    {
        navMeshAgent.isStopped = true;

        FacePlayer();

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            StartFleeing();
        }
    }

    private void CastBuff()
    {
        // Never cast while fleeing.
        if (currentState == State.Fleeing)
            return;

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

    private List<BaseEnemy> FindBuffTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            buffRange
        );

        List<BaseEnemy> targets =
            new List<BaseEnemy>();

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
        /*
         * Immediately stop whatever the support was doing.
         * In particular, it cannot continue casting.
         */
        currentState = State.Fleeing;

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = fleeSpeed;

        SetFurthestFleeDestination();
    }

    private void UpdateFleeing()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = fleeSpeed;

        /*
         * If the player can still see us, find another position
         * that gets us as far away from them as possible.
         */
        if (CanSeePlayer())
        {
            SetFurthestFleeDestination();
        }
        else
        {
            /*
             * Once we've broken line of sight, stop fleeing.
             * We can resume supporting from our new position.
             */
            navMeshAgent.speed =
                baseMoveSpeed * GetSpeedMultiplier();

            searchTimer = searchInterval;

            currentState = State.Searching;
        }
    }

    private void SetFurthestFleeDestination()
    {
        Vector3 bestPosition = transform.position;

        float bestDistance = 
            Vector3.Distance(
                transform.position,
                player.transform.position
            );

        /*
         * Sample positions in a circle around us.
         *
         * We choose the position that puts the support
         * furthest away from the player.
         */
        for (int i = 0; i < fleeSamples; i++)
        {
            Vector3 randomDirection =
                Random.insideUnitSphere * fleeDistance;

            randomDirection.y = 0f;

            if (randomDirection.sqrMagnitude < 0.01f)
                continue;

            Vector3 candidate =
                transform.position + randomDirection;

            if (!NavMesh.SamplePosition(
                candidate,
                out NavMeshHit hit,
                fleeDistance,
                NavMesh.AllAreas))
            {
                continue;
            }

            float distanceFromPlayer =
                Vector3.Distance(
                    hit.position,
                    player.transform.position
                );

            if (distanceFromPlayer > bestDistance)
            {
                bestDistance = distanceFromPlayer;
                bestPosition = hit.position;
            }
        }

        if (bestPosition != transform.position)
        {
            navMeshAgent.SetDestination(bestPosition);
        }
    }
    
    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector3 origin =
            transform.position +
            Vector3.up * visionHeight;

        Vector3 target =
            player.transform.position +
            Vector3.up * visionHeight;

        Vector3 direction =
            target - origin;

        float distance = direction.magnitude;

        if (Physics.Raycast(
                origin,
                direction.normalized,
                out RaycastHit hit,
                distance))
        {
            return hit.transform == player.transform ||
                   hit.transform.IsChildOf(player.transform);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            buffRange
        );

        Gizmos.DrawWireSphere(
            transform.position,
            fleeDistance
        );
    }
}