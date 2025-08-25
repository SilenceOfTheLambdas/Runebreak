using System;
using UnityEngine;

namespace LambdaBT
{
    [Serializable]
    public abstract class LambdaBehaviourTree : MonoBehaviour
    {
        public LambdaBtNode RootNode;
        
        /// <summary>
        /// Construct your behaviour tree here!
        /// <returns><see cref="LambdaBtNode"/>The RootNode of the tree.</returns>
        /// </summary>
        public abstract LambdaBtNode ConstructBehaviourTree();
    }
}
