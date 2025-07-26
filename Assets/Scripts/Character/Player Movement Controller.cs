using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    /// <summary>
    /// Responsible for handling the player movement.
    /// </summary>
    public class PlayerMovementController : MonoBehaviour
    {
        private void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody = GetComponentInChildren<Rigidbody2D>();
            _playerCombatController = GetComponent<PlayerCombatController>();
        }

        private void FixedUpdate()
        {
            // The player cannot move whilst the attack animation is playing
            if (_playerCombatController.IsAttacking != true)
            {
                UpdateMovement(_isPlayerRunning ? runningSpeed : walkingSpeed);
                UpdateOrientation();
            }

            // |=== Falling Gravity Multiplier ===|
            if (IsPlayerGrounded != true)
            {
                // Apply extra gravity when the player is falling
                _rigidbody.linearVelocity +=
                    Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
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
            // Only allow horizontal movement if the player is grounded or moving upward
            var canMove = IsPlayerGrounded || _rigidbody.linearVelocityY > 0;
            
            var targetSpeedX = canMove ?_moveDirection.x * speed : 0f;
            var speedDifferenceX = targetSpeedX - _rigidbody.linearVelocityX;
            var accelerationRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? maximumAcceleration : deceleration;
            
            // Apply more controlled acceleration
            var movementX = speedDifferenceX * accelerationRateX;
            _rigidbody.AddForce(movementX * Vector2.right, ForceMode2D.Force);

            // Limit horizontal speed
            if (Mathf.Abs(_rigidbody.linearVelocityX) > speed)
            {
                _rigidbody.linearVelocityX = Mathf.Sign(_rigidbody.linearVelocityX) * speed;
            }

            // Apply appropriate drag based on state
            _rigidbody.linearDamping = IsPlayerGrounded ? groundLinearDrag : airLinearDrag;

            // Stop minor sliding
            if (IsPlayerGrounded && Mathf.Abs(_moveDirection.x) < 0.01f)
            {
                if (Mathf.Abs(_rigidbody.linearVelocityX) < groundedThreshold)
                {
                    _rigidbody.linearVelocityX = 0f;
                }
            }

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

        
        /// <summary>
        /// Called by the InputAction event system. When the player triggers the jump button, the rigidbodys
        ///  velocity is updated in the Y-direction.
        /// </summary>
        /// <param name="context"></param>
        public void Jump(InputAction.CallbackContext context)
        {
            // Make the character jump when they press the JUMP key.
            if (IsPlayerGrounded && context.performed)
            {
                // Make the character jump
                IsPlayerGrounded = false;
                _rigidbody.linearVelocityY += ((jumpHeight * jumpAcceleration) + Mathf.Abs(_rigidbody.linearVelocity.normalized.x));
                playerMovementAnimator.SetFloat(VelocityX, 0);
                playerMovementAnimator.SetBool(IsJumping, true);
            }
        }

        /// <summary>
        /// Called by the InputAction event system. When player triggers the sprint button, the player character
        /// will run.
        /// </summary>
        /// <param name="context"></param>
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
                {
                    _spriteRenderer.flipX = true;
                    PlayerFacingDirection = Vector2.left;
                }
                else if (_moveDirection.x > 0)
                {
                    _spriteRenderer.flipX = false;
                    PlayerFacingDirection = Vector2.right;
                }
            }
        }
        
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

        public Vector2 PlayerFacingDirection { get; private set; }

        [Header("Movement")]
        
        // [Range(0f, 1f)]
        [SerializeField] private float maximumAcceleration;
        [SerializeField] private float groundLinearDrag = 8f;
        [SerializeField] private float airLinearDrag = 2.5f;
        [SerializeField] private float groundedThreshold = 0.2f;
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
        private PlayerCombatController _playerCombatController;

        #endregion
    }
}