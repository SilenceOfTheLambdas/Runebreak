using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character
{
    public class RPGSystem : MonoBehaviour
    {
        private void Start()
        {
            _currentHealth = maximumHealth;
            CurrentStamina = maximumStamina;
            _currentResolve = maximumResolve;
            _animator = GetComponentInChildren<Animator>();
            
            Assert.IsNotNull(_animator, "The character must have an Animator component as a child.");
        }

        private void Update()
        {
            // Passivly restore stamina
            RestoreStamina(staminaRegenAmount * (Time.deltaTime * staminaRegenRate));
        }

        /// <summary>
        /// Deals x amount of damage to this character. This function will also for player death.
        /// </summary>
        /// <param name="damage">The amount of damage to take.</param>
        public void ReceiveDamage(int damage)
        {
            _currentHealth -= damage;
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
            CurrentStamina -= amount;
        }

        public void RestoreStamina(float amount)
        {
            CurrentStamina += amount;
        }
        
        private readonly static int HasBeenHit = Animator.StringToHash("hasBeenHit");

        [Header("Core Stats")]
        public int maximumHealth = 10;
        public int maximumStamina = 20;
        public int maximumResolve = 10;
        public int currentLevel = 1;

        [Tooltip("(1 unit per x) where x is the value of this variable.")]
        [Range(0.01f, 1f)]
        public float healthRegenRate;

        [Tooltip("(X unit per Y) where Y is the value of this variable and Y is staminaRegenAmount.")]
        [Range(0f, 2f)]
        public float staminaRegenRate;

        [Tooltip("(X unit per Y) where X is the value of this variable and Y is staminaRegenRate.")]
        public float staminaRegenAmount;

        [Tooltip("(1 unit per x) where x is the value of this variable.")]
        [Range(0.01f, 1f)]
        public float resolveRegenRate;
        
        
        public float CurrentStamina { get; private set; }
        private int _currentHealth;
        private int _currentResolve;
        private Animator _animator;
    }
}