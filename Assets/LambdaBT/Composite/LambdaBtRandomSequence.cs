using System.Collections.Generic;
using UnityEngine;

namespace LambdaBT.Composite
{
    /// <summary>
    /// Works the same as <see cref="LambdaBtSequence"/> however this implementation
    /// will perform the sequence in a random order.
    /// </summary>
    public class LambdaBtRandomSequence : LambdaBtNode
    {
        private readonly List<LambdaBtNode> _children;

        public LambdaBtRandomSequence(IEnumerable<LambdaBtNode> children) =>
            _children = new List<LambdaBtNode>(children);

        public override Result ExecuteFrame(float deltaTime, LambdaBlackboard blackboard)
        {
            var isAnyNodeRunning = false;
            for (var i = 0; i < _children.Count; i++)
            {
                var randomIndex = Random.Range(i, _children.Count - 1);
                var node = _children[randomIndex];
                switch (node.ExecuteFrame(deltaTime, blackboard))
                {
                    case Result.Running:
                        isAnyNodeRunning = true;
                        break;
                    case Result.Failure:
                        return Result.Failure;
                    case Result.Success:
                        break;
                    default:
                        return Result.Failure;
                }
            }
            
            return isAnyNodeRunning ? Result.Running : Result.Success;
        }

        public override Result ExecutePhysics(float fixedDeltaTime, LambdaBlackboard blackboard)
        {
            var isAnyNodeRunning = false;
            for (var i = 0; i < _children.Count; i++)
            {
                var randomIndex = Random.Range(i, _children.Count - 1);
                var node = _children[randomIndex];
                switch (node.ExecutePhysics(fixedDeltaTime, blackboard))
                {
                    case Result.Running:
                        isAnyNodeRunning = true;
                        break;
                    case Result.Failure:
                        return Result.Failure;
                    case Result.Success:
                        break;
                    default:
                        return Result.Failure;
                }
            }
            
            return isAnyNodeRunning ? Result.Running : Result.Success;
        }
    }
}