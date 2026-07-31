using UnityEngine;

public interface IHealthRule
{
    public void Damage(float damage);
    public void Die();
    
    public float ReturnHealth();
}
