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

                // // Start a timer
                // _startComboTimer = true;
                // _comboAttackTimer = maxComboAttackTime;
            };

            if (_startComboTimer)
            {
                // Check for input whilst in the combo timer period
                _comboAttackTimer -= Time.deltaTime;
                if (_comboAttackTimer <= 0.001f)
                {
                    _startComboTimer = false;
                    _comboAttackTimer = maxComboAttackTime;
                    animator.SetTrigger(ComboFailed);
                    IsAttacking = false;
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
            if (_rpgSystem.CurrentStamina >= swordAttackStaminaCost && context.performed /*&& IsAttacking == false*/)
            {
                if (_startComboTimer)
                {
                    switch (_swordAttackState)
                    {
                        case SwordAttackState.FirstAttack:
                            _swordAttackState = SwordAttackState.SecondAttack;
                            // DO SECOND THE ATTACK
                            CarryOutSwordAttack(ComboAttack1, swordAttackStaminaCost, SwordAttackState.SecondAttack);
                            return;
                        case SwordAttackState.SecondAttack:
                            _swordAttackState = SwordAttackState.ThirdAttack;
                            // DO THE THIRD ATTACK
                            CarryOutSwordAttack(ComboAttack2, swordAttackStaminaCost, SwordAttackState.ThirdAttack);
                            return;
                        case SwordAttackState.ThirdAttack:
                            _swordAttackState = SwordAttackState.FirstAttack;
                            CarryOutSwordAttack(ComboAttack3, swordAttackStaminaCost, SwordAttackState.FirstAttack);
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
            // Start a timer
            _comboAttackTimer = maxComboAttackTime;
            _startComboTimer = true;
            IsAttacking = true;

            animator.SetTrigger(animationID);
            _swordAttackState = stateToSwitchTo;

            // Expend stamina
            _rpgSystem.ExpendStamina(staminaCost);


            // TODO: DEBUGying out " + stateToSwitchTo);
        }

        /// <summary>
        /// Calculates the amount of damage the player will deal to an enemy based on the state of the attack combo
        /// if the player is in one.
        /// </summary>
        /// <returns>Returns an Int value representing the damage, will default to base sword damage if not in
        /// a combat combo.</returns>
        public int CalculateSwordAttackDamage()
        {
            switch (_swordAttackState)
            {
                case SwordAttackState.None:
                    break;
                case SwordAttackState.FirstAttack:
                    return ComboAttack1Damage;
                case SwordAttackState.SecondAttack:
                    return ComboAttack2Damage;
                case SwordAttackState.ThirdAttack:
                    return ComboAttack3Damage;
            }
            return SwordDamage;
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
        private float _comboAttackTimer;
        private bool _startComboTimer;

        private readonly static int Attack = Animator.StringToHash("swordAttack");
        private readonly static int SwordAttackSpeed = Animator.StringToHash("swordAttackSpeed");
        private readonly static int ComboAttack1 = Animator.StringToHash("combo1");
        private readonly static int ComboAttack2 = Animator.StringToHash("combo2");
        private readonly static int ComboAttack3 = Animator.StringToHash("combo3");
        private readonly static int ComboFailed = Animator.StringToHash("comboFailed");

        [Header("Attack Properties")]
        public bool IsAttacking { get; private set; }

        public float SwordAttackRange { get; private set; }

        public int SwordDamage { get; private set; }

        [Header("Attack Combo")]

        public int ComboAttack1Damage;
        public int ComboAttack2Damage;
        public int ComboAttack3Damage;

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
