using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerCombatController : MonoBehaviour
    {
        public void Start()
        {
            _rpgSystem = GetComponent<RPGSystem>();
            _playerMovementController = GetComponent<PlayerMovementController>();
            animator.SetFloat(SwordAttackSpeed, attackSpeed);
            SwordAttackRange = swordAttackRange;
            
            Assert.IsNotNull(_rpgSystem, "No RPGSystem component found on the player.");
            Assert.IsNotNull(_playerMovementController, "No PlayerMovementController component found on the player.");
        }

        public void LateUpdate()
        {
            PlayerCombatAnimationState.OnCombatAnimationEnd += () =>
            {
                IsAttacking = false;
            };
        }

        private void Update()
        {
            UpdateUIStatusElements();

            // Stamina Drain When Blocking
            if (isBlocking)
                _rpgSystem.ExpendStamina(blockingStaminaDrain * (Time.deltaTime * blockingStaminaDrainRate));
            
            // Exit out of blocking when we run out of stamina
            if (isBlocking && !HasEnoughStaminaToPerformAction(ref blockingStaminaDrain))
            {
                animator.ResetTrigger(Blocking);
                animator.SetBool(IsBlocking, false);
                isBlocking = false;
            }
        }

        private void UpdateUIStatusElements()
        {
            playerStaminaText.text = "Stamina: " + (int)_rpgSystem.CurrentStamina;
        }

        /// <summary>
        /// Called by the InputActions event system.
        /// Triggers an attack and plays corresponding animation only if the player has enough stamina.
        /// </summary>
        /// <param name="context"></param>
        public void StartSwordAttack(InputAction.CallbackContext context)
        {
            if (_rpgSystem.CurrentStamina >= swordAttackStaminaCost && context.performed)
            {
                if (_isWithinComboAttackTimingRange)
                {
                    switch (_nextSwordAttackState)
                    {
                        case SwordAttackState.SecondAttack:
                            // DO THE THIRD ATTACK
                            StartAttackAnimation_SwitchComboState(ComboAttack2, 
                                swordAttackStaminaCost, SwordAttackState.SecondAttack, SwordAttackState.ThirdAttack);
                            return;
                        case SwordAttackState.ThirdAttack:
                            StartAttackAnimation_SwitchComboState(ComboAttack3, 
                                swordAttackStaminaCost, SwordAttackState.ThirdAttack, SwordAttackState.FirstAttack);
                            return;
                    }
                }
                else
                {
                    StartAttackAnimation_SwitchComboState(ComboAttack1, 
                        swordAttackStaminaCost, SwordAttackState.FirstAttack, SwordAttackState.SecondAttack);
                }
            }
        }

        /// <summary>
        /// Checks for a valid enemy within the player's attack range and applies damage if found.
        /// </summary>
        /// <remarks>
        /// This method performs a raycast in the direction the player is facing to detect enemies.
        /// If an enemy is found within range, it applies damage to the enemy and triggers a hit animation.
        /// </remarks>
        /// <returns>
        /// Returns true if a valid enemy was found and damaged, false otherwise.
        /// </returns>
        public bool CheckForValidEnemyInRange_DoDamageToEnemy()
        {
            // TODO: Create properties for these
            var playerMovementController = animator.GetComponentInParent<PlayerMovementController>();
            var playerCombatController = animator.GetComponentInParent<PlayerCombatController>();

            var playerPosition = animator.gameObject.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(playerPosition,
                playerMovementController.PlayerFacingDirection * playerCombatController.SwordAttackRange,
                playerCombatController.SwordAttackRange, LayerMask.GetMask("Hittable"));

            // THIS IS WHERE THE HIT OCCURS
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // DAMAGE IS DONE HERE
                var enemyRPGSystem = hit.collider.GetComponent<RPGSystem>();
                enemyRPGSystem.ReceiveDamage(playerCombatController.CalculateSwordAttackDamage());
                enemyRPGSystem.PlayHittedAnimation();
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes a sword attack, updating the player's state, triggering animations, and managing stamina.
        /// </summary>
        /// <param name="animationID">The ID of the animation to trigger for this attack.</param>
        /// <param name="staminaCost">The amount of stamina to be expended for this attack.</param>
        /// <param name="currentAttackState">The current attack state.</param>
        /// <param name="stateToSwitchTo">The new SwordAttackState to transition to after this attack.</param>
        /// <remarks>
        /// This method starts a combo timer, sets the player's attacking state, triggers the appropriate animation,
        /// updates the sword attack state, and expends the required stamina.
        /// </remarks>
        private void StartAttackAnimation_SwitchComboState(int animationID, int staminaCost, SwordAttackState currentAttackState , SwordAttackState stateToSwitchTo)
        {
            // Trigger the animation
            animator.SetTrigger(animationID);
            
            // Expend stamina
            _rpgSystem.ExpendStamina(staminaCost);
            
            IsAttacking = true;

            _currentAttackState = currentAttackState;
            // Queue the next attack state
            _nextSwordAttackState = stateToSwitchTo;
        }

        /// Starts the blocking behaviour and corresponding animation. This function is called by
        /// the InputManager events system.
        public void StartBlocking(InputAction.CallbackContext context)
        {
            if (context.performed && GetComponent<PlayerMovementController>().IsPlayerGrounded 
                                  && HasEnoughStaminaToPerformAction(ref blockingStaminaDrain))
            {
                animator.SetTrigger(Blocking);
                animator.SetBool(IsBlocking, true);
                isBlocking = true;
            }
            
            if (context.canceled)
            {
                animator.ResetTrigger(Blocking);
                animator.SetBool(IsBlocking, false);
                isBlocking = false;
            }
        }

        public void StartComboAttackTimer()
        {
            _isWithinComboAttackTimingRange = true;
        }

        public void EndComboAttackTimer()
        {
            _isWithinComboAttackTimingRange = false;
            animator.SetTrigger(ComboFailed);
            IsAttacking = false;
        }

        /// <summary>
        /// Calculates the amount of damage the player will deal to an enemy based on the state of the attack combo
        /// if the player is in one. The actual damage done will be the state + 1 e.g. <c>FirstAttack => ComboAttack3Damage </c>.
        /// This is an acyclic system.
        /// </summary>
        /// <returns>Returns an Int value representing the damage, will default to base sword damage if not in
        /// a combat combo.</returns>
        private int CalculateSwordAttackDamage()
        {
            return _currentAttackState switch
            {
                SwordAttackState.FirstAttack => ComboAttack1Damage,
                SwordAttackState.SecondAttack => ComboAttack2Damage,
                SwordAttackState.ThirdAttack => ComboAttack3Damage,
                _ => ComboAttack1Damage
            };
        }

        /// <summary>
        /// Determines if the player has enough stamina to perform an action.
        /// </summary>
        /// <param name="staminaCost">The amount of stamina required to perform the action.</param>
        /// <returns>
        /// Returns <c>true</c> if the player has sufficient stamina to perform the action; otherwise, <c>false</c>.
        /// </returns>
        private bool HasEnoughStaminaToPerformAction(ref float staminaCost)
        {
            if (_rpgSystem.CurrentStamina <= 0)
                return false;

            if (_rpgSystem.CurrentStamina - staminaCost >= 0)
                return true;

            return false;
        }
        
        private enum SwordAttackState
        {
            FirstAttack = 1,
            SecondAttack = 2,
            ThirdAttack = 3
        }

        private SwordAttackState _nextSwordAttackState;
        private SwordAttackState _currentAttackState;
        private PlayerMovementController _playerMovementController;
        private bool _isWithinComboAttackTimingRange;

        [HideInInspector] public bool isBlocking;

        private readonly static int SwordAttackSpeed = Animator.StringToHash("swordAttackSpeed");
        private readonly static int ComboAttack1 = Animator.StringToHash("combo1");
        private readonly static int ComboAttack2 = Animator.StringToHash("combo2");
        private readonly static int ComboAttack3 = Animator.StringToHash("combo3");
        private readonly static int ComboFailed = Animator.StringToHash("comboFailed");
        private readonly static int Blocking = Animator.StringToHash("startBlocking");
        private readonly static int IsBlocking = Animator.StringToHash("isBlocking");

        public bool IsAttacking { get; private set; }

        public float SwordAttackRange { get; private set; }


        [Header("Attack Combo")]
        public int ComboAttack1Damage;
        public int ComboAttack2Damage;
        public int ComboAttack3Damage;

        [Header("Attack Properties")]
        
        [SerializeField] [Tooltip("The amount of stamina that is expended during blocking.")]
        private float blockingStaminaDrain;

        [SerializeField] [Tooltip("The rate at which stamina is drained during blocking.")] [Range(0f, 2f)]
        private float blockingStaminaDrainRate;
        
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
        
                
        [SerializeField]
        private TextMeshProUGUI playerHealthText;

        [SerializeField]
        private TextMeshProUGUI playerStaminaText;
    }
}