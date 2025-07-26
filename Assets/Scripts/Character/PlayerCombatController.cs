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
        }

        public void LateUpdate()
        {
            CheckForAttackAnimationEnd.OnAnimationEnd += () =>
            {
                IsAttacking = false;
            };
        }

        /// <summary>
        /// Called by the InputActions event system.
        /// Triggers an attack and plays corresponding animation only if the player has enough stamina.
        /// </summary>
        /// <param name="context"></param>
        public void SwordAttack(InputAction.CallbackContext context)
        {
            if (_rpgSystem.CurrentStamina >= swordAttackStaminaCost && context.performed
                && IsAttacking == false)
            {
                animator.SetTrigger(Attack);
                IsAttacking = true;
                // Expend stamina
                _rpgSystem.ExpendStamina(swordAttackStaminaCost);
            }
        }
        
        private readonly static int Attack = Animator.StringToHash("swordAttack");
        private readonly static int SwordAttackSpeed = Animator.StringToHash("swordAttackSpeed");

        [Header("Attack Properties")]
        public bool IsAttacking { get; private set; }
        
        [SerializeField]
        private int swordAttackStaminaCost;

        [SerializeField]
        private float attackSpeed;
        
        [Header("Object References")]
        
        [SerializeField] 
        private InputActionReference swordAttackInputAction;

        [SerializeField]
        private Animator animator;
        
        private RPGSystem _rpgSystem;
    }
}
