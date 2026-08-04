using System.Diagnostics;
using UnityEngine;

/// <summary>
/// TODO: Assign all rules once the rules have been made
/// TODO: change variables of strategies
/// TODO: make variables serializable
/// </summary>
public class RuleManager : MonoBehaviour
{
    //current commands
        public IHealthRule CurrentHealthRule;
        public IMovementRule CurrentMovementRule;
        public IReloadRule CurrentReloadRule;
        public IWeaponRule CurrentWeaponRule;
        public IGravityRule CurrentGravityRule;
        
    //shoot Rules
        private IWeaponRule boomerGunRule;
        private IWeaponRule revolverGunRule;
        private IWeaponRule chargeShootRule;
        private IWeaponRule projectileShootRule;
    
    //move Rules
        private IMovementRule boomerMoveRule;
        private IMovementRule moonMoveRule;
        private IMovementRule physicsMoveRule;
        private IMovementRule fastMoveRule;
    
    //reload Rules
        private IReloadRule noReloadRule;
        private IReloadRule revolverReloadRule;
        
    //Health Rules
        private IHealthRule normalHealthRule;
        private IHealthRule shieldHealthRule;
        private IHealthRule segmentHealthRule;
        private IHealthRule rallyHealthRule;
        
    //Gravity Rules
        private IGravityRule normalGravityRule;
        private IGravityRule moonGravityRule;

        [Header("enemy")] 
        [SerializeField] private LayerMask enemyLayerMask;
        
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Camera  playerCamera;

        [SerializeField] private Player player;

        public void SetupStrategies(CharacterController controller, GameObject playerGameObject)
        {
            //create the rule constructors here
            boomerMoveRule = new BoomerMovementStrategy(12f, playerGameObject);
            moonMoveRule = new MoonMovementStrategy(7f, playerGameObject, 5f);
            fastMoveRule = new FastMovementStrategy(20f, playerGameObject, 5f, 50f);
            physicsMoveRule = new PhysicsMovementStrategy(playerGameObject, 20f, 12f, 2f);
            CurrentMovementRule = boomerMoveRule;

            normalGravityRule = new NormalGravityRule();
            moonGravityRule = new MoonGravityRule();
            CurrentGravityRule = normalGravityRule;

            revolverReloadRule = new RevolverReload(6, 1.5f);
            noReloadRule = new NoReloadRule();
            CurrentReloadRule = noReloadRule;

            boomerGunRule = new HitscanGunRule(enemyLayerMask, 25, 0.55f, 25f, playerGameObject);
            revolverGunRule = new HitscanGunRule(enemyLayerMask, 35, 1f, 35f, playerGameObject);
            chargeShootRule = new ChargeGunRule(enemyLayerMask, 10, 100, 0.5f, 3f,playerGameObject);
            projectileShootRule = new ProjectileGunRule(40f, 0.25f, playerCamera, bulletPrefab, enemyLayerMask, player,30f);
            CurrentWeaponRule = boomerGunRule;

            normalHealthRule = new NormalHealthRule(100);
            shieldHealthRule = new ShieldHealthRule(100, 10, 2.5f);
            segmentHealthRule = new SegmentHealthRule(8);
            rallyHealthRule = new PostDamageHealthRule(100, 0.75f);
            CurrentHealthRule = rallyHealthRule;
        }

        public void ChangeRules(LoopData loopData)
        {
            //health rule change
            switch (loopData.currentHealthRule)
            {
                case EHealtRule.Normal :
                    CurrentHealthRule = normalHealthRule;
                    break;
                case EHealtRule.shield :
                    CurrentHealthRule = shieldHealthRule;
                    break;
                case EHealtRule.masks :
                    CurrentHealthRule =  segmentHealthRule;
                    break;
                case EHealtRule.timer :
                    CurrentHealthRule  = rallyHealthRule;
                    break;
            }

            //weapon rules changes
            switch (loopData.currentGunRule)
            {
                case EGunRule.Boomer :
                    CurrentWeaponRule = boomerGunRule;
                    CurrentReloadRule = noReloadRule;
                    break;
                case EGunRule.Energy :
                    CurrentWeaponRule = chargeShootRule;
                    CurrentReloadRule = noReloadRule;
                    break;
                case EGunRule.Projectile :
                    CurrentWeaponRule = projectileShootRule;
                    CurrentReloadRule = revolverReloadRule;
                    break;
                case EGunRule.Revolver :
                    CurrentWeaponRule = revolverGunRule;
                    CurrentReloadRule = revolverReloadRule;
                    break;
            }
            
            //movement rule changes
            switch (loopData.currentMovementRule)
            {
                case EMovementRule.Boomer:
                    CurrentMovementRule = boomerMoveRule;
                    break;
                case EMovementRule.Physics:
                    CurrentMovementRule =  physicsMoveRule;
                    break;
                case EMovementRule.Fast:
                    CurrentMovementRule = fastMoveRule;
                    break;
                case EMovementRule.Moon:
                    CurrentMovementRule = moonMoveRule;
                    break;
            }
            
            //gravity rule changes
            switch (loopData.currentGravityType)
            {
                case EGravityType.normal:
                    CurrentGravityRule = normalGravityRule;
                    break;
                case EGravityType.moon :
                    CurrentGravityRule = moonGravityRule;
                    break;
            }
        }
}
