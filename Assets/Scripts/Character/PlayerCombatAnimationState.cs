using System;
using Character;
using UnityEngine;
namespace Character
{
    public class PlayerCombatAnimationState : StateMachineBehaviour
    {
        public static event Action OnCombatAnimationStart;
        public static event Action OnCombatAnimationEnd;

        private PlayerMovementController _playerMovementController;
        private PlayerCombatController _playerCombatController;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnCombatAnimationStart?.Invoke();

            _playerMovementController = animator.GetComponentInParent<PlayerMovementController>();
            _playerCombatController = animator.GetComponentInParent<PlayerCombatController>();

            var playerPosition = animator.gameObject.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(playerPosition,
                _playerMovementController.PlayerFacingDirection * _playerCombatController.SwordAttackRange,
                _playerCombatController.SwordAttackRange, LayerMask.GetMask("Hittable"));

            // THIS IS WHERE THE HIT OCCURS
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<RPGSystem>()
                    .ReceiveDamage(_playerCombatController.CalculateSwordAttackDamage(), stateInfo.length);
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnCombatAnimationEnd?.Invoke();
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
