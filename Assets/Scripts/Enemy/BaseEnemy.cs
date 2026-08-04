using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    
    [Header("Buffs")]
    private float speedMultiplier = 1f;
    private float damageMultiplier = 1f;

    private Coroutine buffCoroutine;

    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public void ApplyBuff(
        float duration,
        float speedMultiplier,
        float damageMultiplier)
    {
        if (buffCoroutine != null)
        {
            StopCoroutine(buffCoroutine);
        }

        this.speedMultiplier = speedMultiplier;
        this.damageMultiplier = damageMultiplier;

        buffCoroutine = StartCoroutine(BuffTimer(duration));
    }

    private IEnumerator BuffTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        speedMultiplier = 1f;
        damageMultiplier = 1f;

        buffCoroutine = null;
    }
}
