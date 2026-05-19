using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : MonoBehaviour, IDamagable
{
    private int health = 50;
    private int speed = 10;
    private int damage = 5;
    private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    private IDamagable damagable;
    
    [SerializeField] private float hurtCooldown = 1f;

    private float hurtTimer;

    public bool IsActive = false;
    public bool IsDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        damagable = player.GetComponent<IDamagable>();
    }

    private void Update()
    {
        MoveToPosition(player.transform.position);

        hurtTimer -= Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            if (hurtTimer <= 0f)
            {
                damagable.Hurt(damage);
                hurtTimer = hurtCooldown;
            }
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void Hurt(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
