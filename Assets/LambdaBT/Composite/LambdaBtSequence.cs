using System;
using System.Collections.Generic;
using UnityEngine;

namespace LambdaBT.Composite
{
    /// <summary>
    /// Similar to an AND gate; it will only return success if all child nodes return success.
    /// </summary>
    public class LambdaBtSequence : LambdaBtNode
    {
        private readonly List<LambdaBtNode> _children;

        public LambdaBtSequence(IEnumerable<LambdaBtNode> children) => _children = new List<LambdaBtNode>(children);

        public override void Init()
        {
            foreach (var child in _children)
            {
                child.Init();
            }
        }

        public override Result ExecuteFrame(float deltaTime)
        {
            var isAnyNodeRunning = false;
            foreach (var node in _children)
            {
                switch (node.ExecuteFrame(deltaTime))
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

        public override Result ExecutePhysics(float fixedDeltaTime)
        {
            var isAnyNodeRunning = false;
            foreach (var node in _children)
            {
                switch (node.ExecutePhysics(fixedDeltaTime))
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