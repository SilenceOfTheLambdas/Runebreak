using NUnit.Framework;
using UnityEngine;

namespace Character
{
    public class CheckForComboInputEvent : MonoBehaviour
    {
        private PlayerCombatController _playerCombatController;

        private void Start()
        {
            _playerCombatController = GetComponentInParent<PlayerCombatController>();
            
            Assert.IsNotNull(_playerCombatController);
        }

        public void StartComboTimerEvent()
        {
            // Start Combo Timer
            _playerCombatController.StartComboAttackTimer();
        }

        public void EndComboTimerEvent()
        {
            _playerCombatController.EndComboAttackTimer();
        }
    }
}
