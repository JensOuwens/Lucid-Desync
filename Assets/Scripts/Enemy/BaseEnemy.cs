using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected float maxHealth;
    protected float currentHealth;
    
    [SerializeField] protected GameObject player;

    [SerializeField] protected NavMeshAgent navMeshAgent;
 
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void Hurt(float damage)
    {
        currentHealth -= damage;
        
        Debug.Log(this.gameObject.name + " has taken " + damage + " damage");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log(this.gameObject.name + " has died");
        Destroy(gameObject);
    }
}
