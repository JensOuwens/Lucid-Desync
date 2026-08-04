using UnityEngine;

public class PostDamageHealthRule : IHealthRule
{
    private float maxHealth;
    private float currentHealth;
    private float currentGhostHealth;

    private float ghostHealthPercentage;

    public PostDamageHealthRule(float maxHealth, float ghostHealthPercentage)
    {
        this.maxHealth = maxHealth;
        this.ghostHealthPercentage = ghostHealthPercentage;

        currentHealth = maxHealth;
        currentGhostHealth = 0f;
    }

    public void Damage(float damage)
    {
        // Taking damage
        if (damage > 0f)
        {
            currentHealth -= damage;

            // The newest hit replaces the old ghost health
            currentGhostHealth = damage * ghostHealthPercentage;

            if (currentHealth <= 0f)
            {
                currentHealth = 0f;

                Die();
            }
        }

        // Healing / rallying
        else if (damage < 0f)
        {
            float healAmount = Mathf.Min(
                -damage,
                currentGhostHealth
            );

            currentHealth += healAmount;
            currentGhostHealth -= healAmount;

            currentHealth = Mathf.Min(
                currentHealth,
                maxHealth
            );
        }
    }

    public float ReturnHealth()
    {
        return currentHealth;
    }

    public float ReturnGhostHealth()
    {
        return currentGhostHealth;
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}