using UnityEngine;

public class RuleManager : MonoBehaviour
{
    /// <summary>
    /// this need to be strategies instead
    /// </summary>
    
    //current commands
        public IHealthRule CurrentHealthRule;
        public IMovementRule CurrentMovementRule;
        public IReloadRule CurrentReloadRule;
        public IWeaponRule CurrentWeaponRule;
        
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
    

    private void Awake()
    {
        
    }
    
    


}
