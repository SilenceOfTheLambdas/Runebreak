using LambdaBT;
using UnityEngine;
using UnityEngine.Assertions;

namespace NPCs.Enemies.Behaviour_Tree_Nodes
{
    public class ChasePlayer : LambdaBtNode
    {
        public ChasePlayer(LambdaBehaviourTree tree) : base(tree)
        {
        }

        public override void Init()
        {
            _player = Tree.instanceValues.Find(v => v.CompareTag("Player"));
            _enemyRigidbody = Tree.instanceValues.Find(v => v.CompareTag("Enemy"))?.GetComponent<Rigidbody2D>();
            
            // Sanity Checks
            Assert.IsNotNull(_player, "Could not find player game object in Tree instance values.");
            Assert.IsNotNull(_enemyRigidbody, "Could not find Enemy GameObject in Tree instance values.");
            
            _playerRigidbody = _player.GetComponentInChildren<Rigidbody2D>();
            _animator = _enemyRigidbody.GetComponentInChildren<Animator>();
            _enemySprite = _enemyRigidbody.GetComponentInChildren<SpriteRenderer>();
            
            _enemyMovementSpeed = (float) Tree.blackboard.GetValueByName("MovementSpeed");
            _groundedThreshold = (float) Tree.blackboard.GetValueByName("Grounded Threshold");
        }

        public override Result ExecuteFrame(float deltaTime)
        {
            MoveTowardsPlayer(deltaTime);
            
            // Stop movement if player is close enough
            if (Vector2.Distance(_playerRigidbody.position, _enemyRigidbody.position) < 0.1f)
            {
                _enemyRigidbody.linearVelocityX = 0f;
                _animator.SetFloat(VelocityX, 0.0f);
            }
            
            return Result.Running;
        }

        private void MoveTowardsPlayer(float deltaTime)
        {
            var direction = _playerRigidbody.position - _enemyRigidbody.position;
            // _enemyRigidbody.linearVelocityX += direction.normalized.x * (deltaTime * _enemyMovementSpeed);
            _enemyRigidbody.MovePosition(_enemyRigidbody.position + direction.normalized * deltaTime);
            
            // Stop minor sliding
            // if (Mathf.Abs(direction.x) < 0.01f)
            // {
            //     if (Mathf.Abs(_enemyRigidbody.linearVelocityX) < _groundedThreshold)
            //     {
            //         _enemyRigidbody.linearVelocityX = 0f;
            //         _animator.SetFloat(VelocityX, 0.0f);
            //     }
            // }
        
            UpdateMovementAnimation();
            UpdateOrientation(direction.x);
        }
        
        private void UpdateOrientation(float moveDirectionX)
        {
            if (_enemyRigidbody.linearVelocity.magnitude > 0)
            {
                if (moveDirectionX < 0)
                {
                    _enemySprite.flipX = true;
                    _enemyFacingDirection = Vector2.left;
                }
                else if (moveDirectionX > 0)
                {
                    _enemySprite.flipX = false;
                    _enemyFacingDirection = Vector2.right;
                }
            }
        }
        
        private void UpdateMovementAnimation()
        {
            _animator.SetFloat(VelocityX, Mathf.Abs(_enemyRigidbody.linearVelocityX));
        }

        private GameObject _player;
        private Rigidbody2D _playerRigidbody;

        private SpriteRenderer _enemySprite;
        private Rigidbody2D _enemyRigidbody;
        private Animator _animator;
        private Vector2 _enemyFacingDirection;
        
        private float _enemyMovementSpeed;
        private float _groundedThreshold;
        
        private static readonly int VelocityX = Animator.StringToHash("velocityX");
    }
}