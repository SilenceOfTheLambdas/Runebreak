using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CombatAnimatorController))]
    public class RPGSystem : MonoBehaviour
    {
        [Header("Core Stats")]
        public int maximumHealth = 10;
        public int maximumStamina = 20;
        public int maximumResolve = 10;
        public int currentLevel = 1;

        [Tooltip("(1 unit per x) where x is the value of this variable.")]
        [Range(0.01f, 1f)]
        public float healthRegenRate;

        [Tooltip("(1 unit per x) where x is the value of this variable.")]
        [Range(0.01f, 1f)]
        public float staminaRegenRate;

        [Tooltip("(1 unit per x) where x is the value of this variable.")]
        [Range(0.01f, 1f)]
        public float resolveRegenRate;

        private int _currentHealth;
        public int CurrentStamina { get; private set; }
        private int _currentResolve;
        private CombatAnimatorController _combatAnimatorController;

        private void Start()
        {
            _currentHealth = maximumHealth;
            CurrentStamina = maximumStamina;
            _currentResolve = maximumResolve;
            _combatAnimatorController = GetComponent<CombatAnimatorController>();
        }

        /// <summary>
        /// Deals x amount of damage to this character. This function will also for player death.
        /// </summary>
        /// <param name="damage">The amount of damage to take.</param>
        public void ReceiveDamage(int damage)
        {
            _currentHealth -= damage;
            _combatAnimatorController.RecievedHit();
        }

        public void ExpendStamina(int amount)
        {
            CurrentStamina -= amount;
        }
    }
}
