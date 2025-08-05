using Character;
using UnityEngine;
using UnityEngine.Assertions;

public class HittedAnimationEvent : MonoBehaviour
{
    private PlayerCombatController _playerCombatController;

    private void Start()
    {
        _playerCombatController = GetComponentInParent<PlayerCombatController>();
        
        Assert.IsNotNull(_playerCombatController, "The parent Player gameobject must have a PlayerCombatController component.");
    }

    /// <summary>
    /// Triggers a sword attack by checking for valid enemies in range and dealing damage to them.
    /// This method is typically called from animation events during sword attack animations.
    /// </summary>
    public void OnTriggerSwordAttack()
    {
        // Check for a valid enemy and then do damage to the enemy;
        _playerCombatController.CheckForValidEnemyInRange_DoDamageToEnemy();
    }
}