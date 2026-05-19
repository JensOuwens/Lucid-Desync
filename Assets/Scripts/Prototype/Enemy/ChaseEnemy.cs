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

    public bool IsActive = false;
    public bool IsDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    private void Update()
    {
        MoveToPosition(player.transform.position);
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
