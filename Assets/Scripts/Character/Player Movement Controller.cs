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

        #region SERIALIZED_MEMBER_VARIABLES

        [Header("Movement")]
        
        [Range(0f, 1f)]
        [SerializeField] private float maximumAcceleration;
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float runningSpeed;
        [SerializeField] private float rollingSpeed;
        [SerializeField] private int jumpHeight;

        [Header("Object References")]
        [SerializeField] private InputActionReference moveActionInput;

        #endregion

        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody = GetComponentInChildren<Rigidbody2D>();
        }

        private void Update()
        { 
            _moveDirection = moveActionInput.action.ReadValue<Vector2>().normalized;
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            UpdateOrientation();
        }

        /// <summary>
        /// Move this player towards a target position using the moveAction parameter to figure out the
        /// correct speed to use.
        /// </summary>
        /// <param name="moveAction">The type of movement (walking, running, rolling).</param>
        /// <param name="targetPosition">The world position of the final destination.</param>
        private void UpdateMovement()
        {
            _rigidbody.linearVelocity += _moveDirection * maximumAcceleration;
            _rigidbody.linearVelocity.Normalize();
            _rigidbody.linearVelocity *= walkingSpeed;
        }

        private void UpdateOrientation()
        {
            if (_rigidbody.linearVelocity.magnitude > 0)
            {
                _spriteRenderer.flipX = _rigidbody.linearVelocityX < 0;
            }
        }
    }
}
