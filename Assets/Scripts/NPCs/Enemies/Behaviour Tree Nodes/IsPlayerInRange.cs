using LambdaBT;
using UnityEngine;
using UnityEngine.Assertions;

namespace NPCs.Enemies.Behaviour_Tree_Nodes
{
    public class IsPlayerInRange : LambdaBtNode
    {
        public IsPlayerInRange(LambdaBehaviourTree tree) : base(tree)
        {
        }
        
        public override void Init()
        {
            Assert.IsTrue(Tree.blackboard.GetValueByName("ChaseRadius") is float, "Failed to cast ChaseRadius to float");
            _enemyChaseRadius = (float) Tree.blackboard.GetValueByName("ChaseRadius");


            _player = Tree.instanceValues.Find(v => v.tag.Equals("Player"));
            _enemy = Tree.instanceValues.Find(v => v.tag.Equals("Enemy"));
        }

        public override Result ExecuteFrame(float deltaTime)
        {
            // Get player's current position
            _playerPosition = _player.transform.position;
            _enemyPosition = _enemy.transform.position;

            if (IsPlayWithinChaseRadius())
            {
                Debug.Log("Player is within chase radius");
                return Result.Success;
            }
            
            return Result.Failure;
        }

        public bool IsPlayWithinChaseRadius()
        {
            return Vector3.Distance(_playerPosition, _enemyPosition) <= _enemyChaseRadius;
        }
        
        private float _enemyChaseRadius;
        private GameObject _player;
        private GameObject _enemy;
        private Vector2 _playerPosition;
        private Vector2 _enemyPosition;
    }
}