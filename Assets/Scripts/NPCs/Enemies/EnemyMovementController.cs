using Character;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyMovementController : MonoBehaviour
{
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _playerCharacter = GameObject.FindGameObjectWithTag("Player");
        _playerRigidbody = _playerCharacter.GetComponentInChildren<Rigidbody2D>();
        
        Assert.IsNotNull(_rigidbody2D, "Child object must have a rigidbody2D component.");
        Assert.IsNotNull(_spriteRenderer, "Child object must have a sprite renderer component.");
        Assert.IsNotNull(_animator, "Child object must have an Animator component.");
        
        Assert.IsNotNull(_playerCharacter, "Could not find player game object.");
        Assert.IsNotNull(_playerRigidbody, "Could not find player Rigidbody2D component.");
    }

    private void Update()
    {
        if (Vector2.Distance(_playerRigidbody.position, _rigidbody2D.position) > 0.1f)
        {
            MoveTowardsPlayer();
        }
        else if (Vector2.Distance(_playerRigidbody.position, _rigidbody2D.position) < 0.1f)
        {
            _rigidbody2D.linearVelocityX = 0f;
            _animator.SetFloat(VelocityX, 0.0f);
        }
    }

    private void MoveTowardsPlayer()
    {
        var direction = _playerRigidbody.position - _rigidbody2D.position;
        _rigidbody2D.linearVelocityX += direction.normalized.x * (Time.deltaTime * movementSpeed);
        // Stop minor sliding
        if (_isGrounded && Mathf.Abs(direction.x) < 0.01f)
        {
            if (Mathf.Abs(_rigidbody2D.linearVelocityX) < groundedThreshold)
            {
                _rigidbody2D.linearVelocityX = 0f;
                _animator.SetFloat(VelocityX, 0.0f);
            }
        }
        
        UpdateMovementAnimation();
        UpdateOrientation(direction.x);
    }
    
    private void UpdateOrientation(float moveDirectionX)
    {
        if (_rigidbody2D.linearVelocity.magnitude > 0)
        {
            if (moveDirectionX < 0)
            {
                _spriteRenderer.flipX = true;
                _enemyFacingDirection = Vector2.left;
            }
            else if (moveDirectionX > 0)
            {
                _spriteRenderer.flipX = false;
                _enemyFacingDirection = Vector2.right;
            }
        }
    }

    private void UpdateMovementAnimation()
    {
        _animator.SetFloat(VelocityX, Mathf.Abs(_rigidbody2D.linearVelocityX));
    }

    public PlayerMovementController.MoveActionTypes currentMoveAction;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float groundedThreshold;

    private bool _isGrounded = true;
    private Rigidbody2D _playerRigidbody;
    private Vector2 _enemyFacingDirection;
    
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private GameObject _playerCharacter;
    private Animator _animator;
    
    private static readonly int VelocityX = Animator.StringToHash("velocityX");
}
