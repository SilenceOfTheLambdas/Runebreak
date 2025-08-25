using UnityEngine;

namespace LambdaBT
{
    [AddComponentMenu("Lambda BT/Behaviour Tree Runner")]
    public class LambdaBtRunner : MonoBehaviour
    {
        [Header("Place a Behaviour Tree below")]
        [Space] [Space]
        public LambdaBehaviourTree lambdaBehaviourTree;

        [Header("Blackboard")] [Space] 
        [field: SerializeField] public LambdaBlackboard blackboard;

        private void Start()
        {
            if (lambdaBehaviourTree == null) return;
            lambdaBehaviourTree.RootNode = lambdaBehaviourTree.ConstructBehaviourTree();
            lambdaBehaviourTree.RootNode.Init();
            
            blackboard.PrintBlackboardToConsole();
        }

        private void Update()
        {
            lambdaBehaviourTree?.RootNode.ExecuteFrame(Time.deltaTime, blackboard);
        }

        private void FixedUpdate()
        {
            lambdaBehaviourTree?.RootNode.ExecutePhysics(Time.fixedDeltaTime, blackboard);
        }
    }
}
