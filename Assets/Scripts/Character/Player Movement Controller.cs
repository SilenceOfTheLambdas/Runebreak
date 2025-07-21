using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerMovementController : MonoBehaviour
    {
        public enum MoveActionTypes
        {
            Walking,
            Running,
            Rolling
        }

        public static MoveActionTypes MovementType;

        // PUBLIC
        public MoveActionTypes currentMoveAction;
        public bool IsPlayerMoving { get; set; }
        public bool IsPlayerGrounded { get; set; }

        #region SERIALIZED_MEMBER_VARIABLES

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
        [SerializeField] private InputActionReference moveActionInput;

        [SerializeField] private InputActionReference jumpActionInput;

        #endregion

        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private SpriteRenderer _spriteRenderer;
        private float _currentSpeed;

        private void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody = GetComponentInChildren<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            UpdateOrientation();

            // |=== Falling Gravity Multiplier ===|
            if (IsPlayerGrounded != true)
            {
                // Apply extra gravity when player is falling
                _rigidbody.linearVelocity +=
                    Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// Handles the player's movement systems; left, right, and jump.
        /// Called by InputAction Callback.
        /// </summary>
        private void UpdateMovement()
        {
            var targetSpeedX = _moveDirection.x * walkingSpeed;
            var speedDifferenceX = targetSpeedX - _rigidbody.linearVelocityX;
            var accelerationRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? maximumAcceleration : deceleration;
            
            var movementX = Mathf.Clamp(speedDifferenceX, -maximumAcceleration * Time.fixedDeltaTime, accelerationRateX * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX + movementX, _rigidbody.linearVelocityY);
            
            
            if (_rigidbody.linearVelocity.magnitude != 0)
            {
                IsPlayerMoving = true;
                currentMoveAction = MoveActionTypes.Walking;
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
        }

        public void Jump(InputAction.CallbackContext context)
        {
            // Make the character jump when they press the JUMP key.
            if (IsPlayerGrounded)
            {
                // Make the character jump
                IsPlayerGrounded = false;
                _rigidbody.linearVelocityY += ((jumpHeight * jumpAcceleration) + Mathf.Abs(_rigidbody.linearVelocity.normalized.x));
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
