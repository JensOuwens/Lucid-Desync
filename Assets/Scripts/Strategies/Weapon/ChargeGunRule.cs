using UnityEngine;

public class ChargeGunRule : IWeaponRule
{
    private LayerMask enemyLayerMask;
    private float minDamage;
    private float maxDamage;
    private float minChargeTime;
    private float maxChargeTime;

    private GameObject playerGameObject;
    
    private float currentChargeTime;
    private bool charging;
    private float damage;

    public ChargeGunRule(LayerMask enemyLayerMask,  float minDamage, float maxDamage, float minChargeTime, float maxChargeTime, GameObject playerGameObject)
    {
        this.enemyLayerMask = enemyLayerMask;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.minChargeTime = minChargeTime;
        this.maxChargeTime = maxChargeTime;
        this.playerGameObject = playerGameObject;
    }

    
    public bool Press()
    {
        charging = true;
        currentChargeTime = 0f;
        damage = 0f;
        
        return false;
    }

    public void Hold()
    {
        if (!charging)
            return;

        // Increase charge time
        currentChargeTime += Time.deltaTime;

        // Clamp so it never exceeds maximum
        currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);

        // Calculate percentage between min and max charge time
        float chargePercent = Mathf.InverseLerp(
            minChargeTime,
            maxChargeTime,
            currentChargeTime);

        // Calculate damage using the same percentage
        damage = Mathf.Lerp(
            minDamage,
            maxDamage,
            chargePercent);

        //with percentage, later update cool ui stuff
        //Debug.Log($"Charge: {(chargePercent * 100f):F0}%");
        
    }

    public bool Release()
    {
        charging = false;
        
        //check if minimum damage threshold has been reached           
        if (damage < minDamage)
        {
            currentChargeTime = 0f;
            damage = 0f;
            return false;
        }
        
        // Raycast
        if (!Physics.Raycast(
                playerGameObject.transform.position,
                playerGameObject.transform.forward,
                out RaycastHit hitInfo,
                100))
            return true;

        // Check if hit is in layermask
        if (((1 << hitInfo.collider.gameObject.layer) & enemyLayerMask.value) == 0)
            return true;
        

        // Damage
        if (hitInfo.collider.TryGetComponent<IDamagable>(out var damageable))
        {
            damageable.Hurt(damage);
        }
        
        currentChargeTime = 0f;
        damage = 0f;

        return true;
    }
}
