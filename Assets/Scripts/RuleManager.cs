using System.Diagnostics;
using UnityEngine;

/// <summary>
/// TODO: Assign all rules once the rules have been made
/// TODO: change variables of strategies
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
        private IWeaponRule hitscanShootRule;
        private IWeaponRule chargeShootRule;
        private IWeaponRule projectileShootRule;
        private IWeaponRule swordShootRule;
    
    //move Rules
        private IMovementRule boomerMoveRule;
        private IMovementRule moonMoveRule;
        private IMovementRule physicsMoveRule;
        private IMovementRule fastMoveRule;
    
    //reload Rules
        private IReloadRule noReloadRule;
        private IReloadRule revolverReloadRule;
        private IReloadRule swordReloadRule;
        
    //Health Rules
        private IHealthRule normalHealthRule;
        private IHealthRule shieldHealthRule;
        private IHealthRule maskHealthRule;
        private IHealthRule timerHealthRule;
        
    //Gravity Rules
        private IGravityRule normalGravityRule;
        private IGravityRule moonGravityRule;

        public void SetupStrategies(CharacterController controller, GameObject playerGameObject)
        {
            //create the rule constructors here
            boomerMoveRule = new BoomerMovementStrategy(12f, playerGameObject);
            moonMoveRule = new MoonMovementStrategy(7f, playerGameObject, 5f);
            fastMoveRule = new FastMovementStrategy(20f, playerGameObject, 5f, 50f);
            physicsMoveRule = new PhysicsMovementStrategy(playerGameObject, 20f, 12f, 2f);
            CurrentMovementRule = physicsMoveRule;

            normalGravityRule = new NormalGravityRule();
            moonGravityRule = new MoonGravityRule();
            CurrentGravityRule = normalGravityRule;
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
                    CurrentHealthRule =  maskHealthRule;
                    break;
                case EHealtRule.timer :
                    CurrentHealthRule  = timerHealthRule;
                    break;
            }

            //weapon rules changes
            switch (loopData.currentGunRule)
            {
                case EGunRule.Boomer :
                    CurrentWeaponRule = hitscanShootRule;
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
                    CurrentWeaponRule = hitscanShootRule;
                    CurrentReloadRule = revolverReloadRule;
                    break;
                case EGunRule.Sword :
                    CurrentWeaponRule = swordShootRule;
                    CurrentReloadRule = swordReloadRule;
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
