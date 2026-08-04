using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : BaseEnemy
{
    private enum State
    {
        Chasing,
        PreparingCharge,
        Charging,
        Attacking,
        Recovering
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float attackRange = 2f;

    [Header("Charge")]
    [SerializeField] private float chargeRange = 8f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargePreparationTime = 0.75f;
    [SerializeField] private float chargeDuration = 1.5f;
    [SerializeField] private float chargeDamage = 30f;

    [Header("Melee")]
    [SerializeField] private float meleeDamage = 40f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Recovery")]
    [SerializeField] private float recoveryTime = 1f;

    private State currentState;

    private float stateTimer;
    private float attackTimer;

    private Vector3 chargeDirection;

    private void Start()
    {
        navMeshAgent.speed = moveSpeed;

        currentState = State.Chasing;
    }

    private void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Chasing:
                UpdateChasing();
                break;

            case State.PreparingCharge:
                UpdatePreparingCharge();
                break;

            case State.Charging:
                UpdateCharging();
                break;

            case State.Attacking:
                UpdateAttacking();
                break;

            case State.Recovering:
                UpdateRecovering();
                break;
        }
    }

    private void UpdateChasing()
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.speed =
            moveSpeed * GetSpeedMultiplier();

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= attackRange)
        {
            currentState = State.Attacking;
            return;
        }

        if (distance <= chargeRange)
        {
            StartCharge();
            return;
        }

        navMeshAgent.SetDestination(
            player.transform.position
        );
    }

    private void StartCharge()
    {
        currentState = State.PreparingCharge;

        stateTimer = chargePreparationTime;

        navMeshAgent.isStopped = true;

        FacePlayer();
    }

    private void UpdatePreparingCharge()
    {
        navMeshAgent.isStopped = true;

        FacePlayer();

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            BeginCharging();
        }
    }

    private void BeginCharging()
    {
        currentState = State.Charging;

        stateTimer = chargeDuration;

        navMeshAgent.isStopped = false;

        navMeshAgent.speed =
            chargeSpeed * GetSpeedMultiplier();

        // Lock in the direction of the charge.
        chargeDirection =
            player.transform.position -
            transform.position;

        chargeDirection.y = 0f;

        if (chargeDirection.sqrMagnitude < 0.01f)
        {
            chargeDirection = transform.forward;
        }

        chargeDirection.Normalize();

        Vector3 chargeTarget =
            transform.position +
            chargeDirection * 20f;

        navMeshAgent.SetDestination(
            chargeTarget
        );
    }

    private void UpdateCharging()
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.speed =
            chargeSpeed * GetSpeedMultiplier();

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            EndCharge();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= attackRange)
        {
            ChargeHit();
            EndCharge();
        }
    }

    private void ChargeHit()
    {
        if (player.TryGetComponent<IDamagable>(
            out var damageable))
        {
            float finalDamage =
                chargeDamage *
                GetDamageMultiplier();

            damageable.Hurt(finalDamage);
        }
    }

    private void EndCharge()
    {
        navMeshAgent.isStopped = true;

        navMeshAgent.speed =
            moveSpeed * GetSpeedMultiplier();

        stateTimer = recoveryTime;

        currentState = State.Recovering;
    }

    private void UpdateAttacking()
    {
        navMeshAgent.isStopped = true;

        FacePlayer();

        if (attackTimer > 0f)
            return;

        MeleeAttack();

        attackTimer = attackCooldown;

        stateTimer = recoveryTime;

        currentState = State.Recovering;
    }

    private void MeleeAttack()
    {
        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance > attackRange)
            return;

        if (player.TryGetComponent<IDamagable>(
            out var damageable))
        {
            float finalDamage =
                meleeDamage *
                GetDamageMultiplier();

            damageable.Hurt(finalDamage);
        }
    }

    private void UpdateRecovering()
    {
        navMeshAgent.isStopped = true;

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            currentState = State.Chasing;
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
}