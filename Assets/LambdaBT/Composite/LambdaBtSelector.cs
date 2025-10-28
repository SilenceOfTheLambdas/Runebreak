using System.Collections.Generic;
using UnityEngine;

namespace LambdaBT.Composite
{
    /// <summary>
    /// Similar to an OR gate; will return Result.Success if ANY of the child nodes return Result.Success.
    /// </summary>
    public class LambdaBtSelector : LambdaBtNode
    {
        private readonly List<LambdaBtNode> _children;

        public LambdaBtSelector(IEnumerable<LambdaBtNode> children) => _children = new List<LambdaBtNode>(children);

        public override void Init()
        {
            foreach (var child in _children)
            {
                child.Init();
            }
        }

        public override Result ExecuteFrame(float deltaTime)
        {
            foreach (var node in _children)
            {
                Debug.Log("Running: " + node);
                switch (node.ExecuteFrame(deltaTime))
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

        public override Result ExecutePhysics(float fixedDeltaTime)
        {
            foreach (var node in _children)
            {
                switch (node.ExecutePhysics(fixedDeltaTime))
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