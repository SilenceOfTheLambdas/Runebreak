using UnityEngine;

namespace LambdaBT
{
    [AddComponentMenu("Lambda BT/Behaviour Tree Runner")]
    public class LambdaBtRunner : MonoBehaviour
    {
        [Header("Place a Behaviour Tree below")]
        [Space] [Space]
        public LambdaBehaviourTree lambdaBehaviourTree;

        private void Start()
        {
            if (lambdaBehaviourTree == null) return;
            
            // Construct the behaviour tree and initialize the root node.
            lambdaBehaviourTree.RootNode = lambdaBehaviourTree.ConstructBehaviourTree();
            
            // Initialize the root node to start the behaviour tree execution.
            lambdaBehaviourTree?.RootNode.Init();
        }

        private void Update()
        {
            var status = lambdaBehaviourTree?.RootNode.ExecuteFrame(Time.deltaTime);
            Debug.Log($"Current State: {lambdaBehaviourTree?.RootNode} :> {status}");
        }

        private void FixedUpdate()
        {
            lambdaBehaviourTree?.RootNode.ExecutePhysics(Time.fixedDeltaTime);
        }
    }
}
