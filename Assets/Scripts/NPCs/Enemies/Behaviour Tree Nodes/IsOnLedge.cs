using LambdaBT;
using UnityEngine;
using UnityEngine.Assertions;

namespace NPCs.Enemies.Behaviour_Tree_Nodes
{
    public class IsOnLedge : LambdaBtNode
    {
        public override void Init()
        {
            Assert.IsTrue(Tree.blackboard.GetValueByName("Edge Detection Length") is float,
                "Failed to cast Edge Detection Length to float");
            Assert.IsTrue(Tree.blackboard.GetValueByName("Number of Edge Detection Rays") is float,
                "Failed to cast Number of Edge Detection Rays to float");
            
            _enemyEdgeDetectionDistance = (float) Tree.blackboard.GetValueByName("Edge Detection Length");
            _numberOfEdgeDetectionRays = (int) Tree.blackboard.GetValueByName("Number of Edge Detection Rays");
            
            _enemy = Tree.instanceValues.Find(v => v.tag.Equals("Enemy"));
        }

        public override Result ExecuteFrame(float deltaTime)
        {
            return PerformRaycasts() ? Result.Success : Result.Failure;
        }

        /// <summary>
        /// Performs raycasts to check if the enemy is on a ledge.
        /// </summary>
        /// <returns>True if the enemy is on a ledge.</returns>
        private bool PerformRaycasts()
        {
            var raycastSpacing = _enemyEdgeDetectionDistance / _numberOfEdgeDetectionRays;
            bool hasFirstRaycastHitPlatform = false;
            for (int i = 1; i <= _numberOfEdgeDetectionRays; i++)
            {
                var rayOrigin = (Vector2) _enemy.transform.position + (raycastSpacing * i) * Vector2.right;
                var rayDirection = (Vector2)_enemy.transform.position * Vector2.down;
                var ray = new Ray(rayOrigin, rayDirection);

                if (Physics.Raycast(ray, out var hitInfo, _enemyEdgeDetectionDistance))
                {
                    // Check to see if the first raycast hit the platform
                    if (i == 1)
                    {
                        if (hitInfo.collider.gameObject)
                        {
                            hasFirstRaycastHitPlatform = true;
                        }
                    }
                    else if (hasFirstRaycastHitPlatform)
                    {
                        // Now check to see if any subsequent raycasts DO NOT hit anything
                        if (hitInfo.collider == null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        
        private float _enemyEdgeDetectionDistance;
        private float _numberOfEdgeDetectionRays;
        private GameObject _enemy;
    }
}