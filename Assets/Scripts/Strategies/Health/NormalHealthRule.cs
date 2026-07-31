using UnityEngine;

/// <summary>
/// TODO: figure out death
/// </summary>
public class NormalHealthRule : IHealthRule
{
    private float maxHealth;
    private float currentHealth;

    public NormalHealthRule(float maxHealth)
    {
        this.maxHealth = maxHealth;
        
        currentHealth = this.maxHealth;
    }
    
    public void Damage()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public float ReturnHealth()
    {
        return currentHealth;
    }
}
