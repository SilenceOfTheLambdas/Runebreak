using LambdaBT;
using UnityEngine;

namespace NPCs.Enemies.Behaviour_Tree_Nodes
{
    public class IsPlayerInRange : LambdaBtNode
    {
        public override Result ExecuteFrame(float deltaTime, LambdaBlackboard blackboard)
        {
            Debug.Log(blackboard.GetValueByName("Test Value"));
            return Result.Running;
        }
    }
}