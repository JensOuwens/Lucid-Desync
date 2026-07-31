using UnityEngine;

public class ShieldHealthRule : IHealthRule
{
    float currentHealth;
    float maxHealth;
    private float shieldRechargeCooldown;
    private float shieldRechargeRate;
    
    private bool isShieldRecharging;
    
    public float RechargeRate => shieldRechargeRate;
    public float RechargeCooldown => shieldRechargeCooldown;
    public float MaxHealth => maxHealth;

    public ShieldHealthRule(float maxHealth, float shieldRechargeRate, float shieldRechargeCooldown)
    {
        this.maxHealth = maxHealth;
        this.shieldRechargeRate = shieldRechargeRate;
        this.shieldRechargeCooldown = shieldRechargeCooldown;
        
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        isShieldRecharging = false;
        
        currentHealth -= damage;
        
        if (currentHealth <= 0 && !isShieldRecharging)
        {
            currentHealth = 0;
            Die();
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
    
    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
