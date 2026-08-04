using UnityEngine;

public class HitscanGunRule : IWeaponRule
{
    private LayerMask enemyLayerMask;

    private float damage;
    private float fireRate;
    private float rallyAmount;

    private GameObject playerGameObject;
    private Player player;

    private float nextFireTime;

    public HitscanGunRule(
        LayerMask layerMask,
        float damage,
        float fireRate,
        float rallyAmount,
        GameObject playerObject)
    {
        enemyLayerMask = layerMask;

        this.damage = damage;
        this.fireRate = fireRate;
        this.rallyAmount = rallyAmount;

        playerGameObject = playerObject;
        player = playerObject.GetComponent<Player>();
    }

    public bool Press()
    {
        // Check cooldown first
        if (Time.time < nextFireTime)
            return false;

        nextFireTime = Time.time + fireRate;

        // Raycast
        if (!Physics.Raycast(
                playerGameObject.transform.position,
                playerGameObject.transform.forward,
                out RaycastHit hitInfo,
                100f))
        {
            return true;
        }

        // Check if hit is in layer mask
        if (((1 << hitInfo.collider.gameObject.layer) & enemyLayerMask.value) == 0)
        {
            return true;
        }

        // Damage enemy
        if (hitInfo.collider.TryGetComponent<IDamagable>(out var damageable))
        {
            damageable.Hurt(damage);

            // Rally only if PostDamageHealthRule is active
            if (player.GetHealthRule() is PostDamageHealthRule)
            {
                player.Hurt(-rallyAmount);
            }
        }

        return true;
    }

    public void Hold()
    {
        // Do nothing
    }

    public bool Release()
    {
        return false;
    }
}