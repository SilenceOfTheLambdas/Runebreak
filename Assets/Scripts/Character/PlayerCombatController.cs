using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerCombatController : MonoBehaviour
    {
        public void Start()
        {
            _rpgSystem = GetComponent<RPGSystem>();
            animator.SetFloat(SwordAttackSpeed, attackSpeed);
            SwordAttackRange = swordAttackRange;
            SwordDamage = swordDamage;
        }

        public void LateUpdate()
        {
            PlayerCombatAnimationState.OnCombatAnimationEnd += () =>
            {
                IsAttacking = false;
                
                // Start a timer
                _startComboTimer = true;
                _comboAttackTimer = maxComboAttackTime;
            };

            if (_startComboTimer)
            {
                // Check for input whilst in the combo timer period
                _comboAttackTimer -= Time.deltaTime;
                if (_comboAttackTimer <= 0.001f)
                {
                    _startComboTimer = false;
                    _comboAttackTimer = maxComboAttackTime;
                }
            }
            
        }

        /// <summary>
        /// Called by the InputActions event system.
        /// Triggers an attack and plays corresponding animation only if the player has enough stamina.
        /// </summary>
        /// <param name="context"></param>
        public void SwordAttack(InputAction.CallbackContext context)
        {
            if (_rpgSystem.CurrentStamina >= swordAttackStaminaCost && context.performed && IsAttacking == false)
            {
                if (_startComboTimer)
                {
                    switch (_swordAttackState)
                    {
                        case SwordAttackState.FirstAttack:
                            _swordAttackState = SwordAttackState.SecondAttack;
                            // DO SECOND THE ATTACK
                            CarryOutSwordAttack(Attack, swordAttackStaminaCost, SwordAttackState.SecondAttack);
                            return;
                        case SwordAttackState.SecondAttack:
                            _swordAttackState = SwordAttackState.ThirdAttack;
                            // DO THE THIRD ATTACK
                            CarryOutSwordAttack(Attack, swordAttackStaminaCost, SwordAttackState.ThirdAttack);
                            return;
                        case SwordAttackState.ThirdAttack:
                            _swordAttackState = SwordAttackState.FirstAttack;
                            CarryOutSwordAttack(Attack, swordAttackStaminaCost, SwordAttackState.FirstAttack);
                            return;
                            
                    }
                }
                else // FIRST ATTACK
                {
                    _swordAttackState = SwordAttackState.None;
                    CarryOutSwordAttack(Attack, swordAttackStaminaCost, SwordAttackState.FirstAttack);
                }
            }
        }

        private void CarryOutSwordAttack(int animationID, int staminaCost, SwordAttackState stateToSwitchTo)
        {
            animator.SetTrigger(animationID);
            IsAttacking = true;
            _swordAttackState = stateToSwitchTo;

            // Expend stamina
            _rpgSystem.ExpendStamina(staminaCost);
            
            Debug.Log("Carrying out " + stateToSwitchTo.ToString());
        }

        private enum SwordAttackState
        {
            // We have not attacked yet.
            None,
            FirstAttack,
            SecondAttack,
            ThirdAttack
        }

        private SwordAttackState _swordAttackState;
        private float _comboAttackTimer = 0f;
        private bool _startComboTimer = false;
        
        private readonly static int Attack = Animator.StringToHash("swordAttack");
        private readonly static int SwordAttackSpeed = Animator.StringToHash("swordAttackSpeed");

        [Header("Attack Properties")]
        public bool IsAttacking { get; private set; }
        
        public float SwordAttackRange { get; private set; }
        
        public int SwordDamage { get; private set; }
        
        [SerializeField]
        private int swordAttackStaminaCost;

        [SerializeField]
        private float attackSpeed;

        [SerializeField]
        private float maxComboAttackTime;

        [SerializeField]
        private float swordAttackRange;
        
        [SerializeField]
        private int swordDamage;
        
        [Header("Object References")]
        
        [SerializeField] 
        private InputActionReference swordAttackInputAction;

        [SerializeField]
        private Animator animator;
        
        private RPGSystem _rpgSystem;
    }
}
