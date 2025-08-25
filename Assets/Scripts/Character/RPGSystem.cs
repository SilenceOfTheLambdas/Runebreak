using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Character
{
    public class RPGSystem : MonoBehaviour
    {
        private void Start()
        {
            currentHealth = maximumHealth;
            currentStamina = maximumStamina;
            _currentResolve = maximumResolve;
            _animator = GetComponentInChildren<Animator>();
            
            Assert.IsNotNull(_animator, "The character must have an Animator component as a child.");
        }

        private void Update()
        {
            #region Passive Health and Stamina Regeneration
            
            _staminaRegenTimer += Time.deltaTime;

            if (_staminaRegenTimer >= staminaRegenRate)
            {
                if (currentStamina < maximumStamina)
                    RestoreStamina(staminaRegenAmount);
                _staminaRegenTimer = 0f;
            }

            _healthRegenTimer += Time.deltaTime;

            if (_healthRegenTimer >= healthRegenRate)
            {
                if (currentHealth < maximumHealth)
                    RestoreHealth(healthRegenAmount);
                _healthRegenTimer = 0f;
            }

            #endregion
        }


        /// <summary>
        /// Deals x amount of damage to this character. This function will also for player death.
        /// </summary>
        /// <param name="damage">The amount of damage to take.</param>
        public void ReceiveDamage(int damage)
        {
            currentHealth -= damage;
        }

        /// <summary>
        /// Plays the animation for when the character is hit.
        /// </summary>
        /// <remarks>
        /// This method triggers the "hasBeenHit" animation using the character's animator component.
        /// </remarks>
        public void PlayHittedAnimation()
        {
            _animator.SetTrigger(HasBeenHit);
        }

        public void ExpendStamina(float amount)
        {
            currentStamina -= (int) amount;
        }

        /// <summary>
        /// Restores a specified amount of stamina to the character, ensuring it does not exceed the maximum stamina.
        /// </summary>
        /// <param name="amount">The amount of stamina to restore.</param>
        private void RestoreStamina(float amount)
        {
            if (currentStamina + amount > maximumStamina)
            {
                currentStamina = maximumStamina;
                return;
            }
            
            currentStamina += (int) amount;
        }
        
        /// <summary>
        /// Restores a specified amount of health to the character, ensuring it does not exceed the maximum health.
        /// </summary>
        /// <param name="amount">The amount of health to restore.</param>
        private void RestoreHealth(float amount)
        {
            if (currentHealth + amount > maximumHealth)
            {
                currentHealth = maximumHealth;
                return;
            }
            
            currentHealth += (int) amount;
        }
        
        private static readonly int HasBeenHit = Animator.StringToHash("hasBeenHit");

        [Header("Core Stats")]
        public int maximumHealth = 10;
        public int maximumStamina = 20;
        public int maximumResolve = 10;
        public int currentLevel = 1;
        
        [field: SerializeField] public int currentStamina { get; private set; }
        
        [field: SerializeField] public int currentHealth { get; private set; }

        [Space][Header("Regeneration Properties")]
        [Tooltip("(X unit per Y) where Y is the value of this variable.")]
        [Range(0f, 2f)]
        public float healthRegenRate;
        
        [Tooltip("The amount of health to regen every n seconds; where n is healthRegenRate.")]
        [SerializeField] private float healthRegenAmount;

        [Tooltip("(X unit per Y) where Y is the value of this variable and Y is staminaRegenAmount.")]
        [Range(0f, 2f)]
        public float staminaRegenRate;

        [Tooltip("(X unit per Y) where X is the value of this variable and Y is staminaRegenRate.")]
        public float staminaRegenAmount;

        [Tooltip("(1 unit per x) where x is the value of this variable.")]
        [Range(0.01f, 1f)]
        public float resolveRegenRate;

        private int _currentResolve;
        private Animator _animator;
        private float _staminaRegenTimer = 0f;
        private float _healthRegenTimer = 0f;
    }
}