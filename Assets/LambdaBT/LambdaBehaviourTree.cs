using System;
using System.Collections.Generic;
using UnityEngine;

namespace LambdaBT
{
    [Serializable]
    public abstract class LambdaBehaviourTree : MonoBehaviour
    {
        public LambdaBtNode RootNode;
        public LambdaBlackboard blackboard;
        public List<GameObject> instanceValues;

        /// <summary>
        /// Construct your behaviour tree here!
        /// <returns><see cref="LambdaBtNode"/>The RootNode of the tree.</returns>
        /// </summary>
        public abstract LambdaBtNode ConstructBehaviourTree();
    }
}
