using UnityEngine;

namespace LambdaBT
{
    /// <summary>
    /// Represents a behaviour node, all nodes extend this.
    /// </summary>
    public abstract class LambdaBtNode
    {
        public enum Result
        {
            /// <summary>
            /// This node is still running its behaviour.
            /// </summary>
            Running,
            
            /// <summary>
            /// This node has failed.
            /// </summary>
            Failure,
            
            /// <summary>
            /// This node has succeeded.
            /// </summary>
            Success
        }

        /// <summary>
        /// Place any initialisation code here. This method is called before <see cref="ExecuteFrame"/> or <see cref="ExecutePhysics"/>.
        /// </summary>
        /// <returns><see cref="Result"/></returns>
        public virtual void Init()
        {
        }

        /// <summary>
        /// This method is executed every frame.
        /// </summary>
        /// <param name="deltaTime">Time.deltaTime</param>
        /// <param name="blackboard">Blackboard scriptable object that holds variables you may need.</param>
        /// <returns>Returns a <see cref="Result"/></returns>
        public virtual Result ExecuteFrame(float deltaTime, LambdaBlackboard blackboard)
        {
            return Result.Failure;
        }

        /// <summary>
        /// This method is executed on every physics update.
        /// </summary>
        /// <param name="fixedDeltaTime">Time.fixedDeltaTime</param>
        /// <param name="blackboard"></param>
        /// <returns>Returns a <see cref="Result"/></returns>
        public virtual Result ExecutePhysics(float fixedDeltaTime, LambdaBlackboard blackboard)
        {
            return Result.Failure;
        }

        /// <summary>
        /// This method is called when this node has finished executing.
        /// </summary>
        public virtual void Exit() { }
    }
}
