using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace LambdaBT.Composite
{
    /// <summary>
    /// Works the same as <see cref="LambdaBtSelector"/> however this implementation will execute each child node
    /// in a random order.
    /// </summary>
    public class LambdaBtRandomSelector : LambdaBtNode
    {
        private readonly List<LambdaBtNode> _children;

        public LambdaBtRandomSelector(IEnumerable<LambdaBtNode> children)
        {
            _children = new List<LambdaBtNode>(children);
        }
        
        public override Result ExecuteFrame(float deltaTime, LambdaBlackboard blackboard)
        {
            for (var i = 0; i < _children.Count; i++)
            {
                var randomIndex = Random.Range(i, _children.Count - 1);
                var node = _children[randomIndex];
                switch (node.ExecuteFrame(deltaTime, blackboard))
                {
                    case Result.Running:
                        return Result.Running;
                    case Result.Failure:
                        break;
                    case Result.Success:
                        return Result.Success;
                    default:
                        return Result.Failure;
                }
            }

            // We only get here is NONE of the child nodes return Success.
            return Result.Failure;
        }

        public override Result ExecutePhysics(float fixedDeltaTime, LambdaBlackboard blackboard)
        {
            for (var i = 0; i < _children.Count; i++)
            {
                var randomIndex = Random.Range(i, _children.Count - 1);
                var node = _children[randomIndex];
                switch (node.ExecutePhysics(fixedDeltaTime, blackboard))
                {
                    case Result.Running:
                        return Result.Running;
                    case Result.Failure:
                        break;
                    case Result.Success:
                        return Result.Success;
                    default:
                        return Result.Failure;
                }
            }

            // We only get here is NONE of the child nodes return Success.
            return Result.Failure;
        }
    }
}