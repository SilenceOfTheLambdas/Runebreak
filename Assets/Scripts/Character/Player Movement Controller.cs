using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Variables
        
        public enum MoveActionTypes
        {
            Walking,
            Running,
            Rolling
        }

        public static MoveActionTypes MovementType;
        
        private readonly static int VelocityX = Animator.StringToHash("VelocityX");
        private readonly static int DecelerationX = Animator.StringToHash("DecelerationX");
        private readonly static int IsRunning = Animator.StringToHash("isRunning");
        private readonly static int IsJumping = Animator.StringToHash("isJumping");
        private readonly static int VelocityY = Animator.StringToHash("VelocityY");
        private readonly static int Speed = Animator.StringToHash("Speed");

        // PUBLIC
        public MoveActionTypes currentMoveAction;
        public bool IsPlayerMoving { get; set; }
        public bool IsPlayerGrounded { get; set; }

        [Header("Movement")]
        
        // [Range(0f, 1f)]
        [SerializeField] private float maximumAcceleration;

        [SerializeField] private float deceleration;
        [SerializeField] private float jumpAcceleration;
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float runningSpeed;
        [SerializeField] private float rollingSpeed;
        [SerializeField] private int jumpHeight;
        [SerializeField] private float fallMultiplier;

        [Header("Object References")]
        [SerializeField] private Animator playerMovementAnimator;
        [SerializeField] private InputActionReference moveActionInput;
        [SerializeField] private InputActionReference jumpActionInput;
        [SerializeField] private InputActionReference sprintActionInput;

        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private SpriteRenderer _spriteRenderer;
        private float _currentSpeed;
        private bool _isPlayerRunning;
        
        #endregion

        private void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody = GetComponentInChildren<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            UpdateMovement(_isPlayerRunning ? runningSpeed : walkingSpeed);
            UpdateOrientation();

            // |=== Falling Gravity Multiplier ===|
            if (IsPlayerGrounded != true)
            {
                // Apply extra gravity when player is falling
                _rigidbody.linearVelocity +=
                    Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            playerMovementAnimator.SetFloat(VelocityY, _rigidbody.linearVelocity.normalized.y);
            
            // Update player movemen animations
            if (_rigidbody.linearVelocityX == 0)
            {
                playerMovementAnimator.SetFloat(Speed, 1f);
            }
            else
            {
                playerMovementAnimator.SetFloat(Speed, Mathf.Abs(_rigidbody.linearVelocity.x) * 0.5f); //TODO: Magic Number :(
            }
            playerMovementAnimator.SetFloat(VelocityX, Mathf.Abs(_rigidbody.linearVelocity.x));
        }

        /// <summary>
        /// Handles the player's movement systems; left, right, and jump.
        /// Called by InputAction Callback.
        /// </summary>
        private void UpdateMovement(float speed)
        {
            var targetSpeedX = _moveDirection.x * speed;
            var speedDifferenceX = targetSpeedX - _rigidbody.linearVelocityX;
            var accelerationRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? maximumAcceleration : deceleration;
            
            var movementX = Mathf.Clamp(speedDifferenceX, -maximumAcceleration * Time.fixedDeltaTime, accelerationRateX * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX + movementX, _rigidbody.linearVelocityY);
            
            if (_rigidbody.linearVelocity.magnitude != 0)
            {
                IsPlayerMoving = true;
                currentMoveAction = MoveActionTypes.Walking;
            }

            if (IsPlayerGrounded)
            {
                playerMovementAnimator.SetBool(IsJumping, false);
                

                if (_moveDirection.magnitude == 0)
                {
                    playerMovementAnimator.SetFloat(DecelerationX, Mathf.Abs(_rigidbody.linearVelocity.x));
                }
            }
        }

        /// <summary>
        /// Called by the InputAction callback. Updates the _moveDirection variable whenever the player
        /// activates the Jump action.
        /// </summary>
        /// <param name="context"></param>
        public void UpdateMoveDirection(InputAction.CallbackContext context)
        {
            _moveDirection = moveActionInput.action.ReadValue<Vector2>().normalized;
            playerMovementAnimator.SetFloat(DecelerationX, 0);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            // Make the character jump when they press the JUMP key.
            if (IsPlayerGrounded)
            {
                // Make the character jump
                IsPlayerGrounded = false;
                _rigidbody.linearVelocityY += ((jumpHeight * jumpAcceleration) + Mathf.Abs(_rigidbody.linearVelocity.normalized.x));
                playerMovementAnimator.SetFloat(VelocityX, 0);
                playerMovementAnimator.SetBool(IsJumping, true);
            }
        }

        public void Sprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                playerMovementAnimator.SetBool(IsRunning, true);
                _isPlayerRunning = true;
            }

            if (context.canceled)
            {
                playerMovementAnimator.SetBool(IsRunning, false);
                _isPlayerRunning = false;
            }
        }

        private void UpdateOrientation()
        {
            if (_rigidbody.linearVelocity.magnitude > 0)
            {
                if (_moveDirection.x < 0)
                    _spriteRenderer.flipX = true;
                else if (_moveDirection.x > 0)
                    _spriteRenderer.flipX = false;
            }
        }
    }
}
