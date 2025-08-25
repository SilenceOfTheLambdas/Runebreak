using System.Collections.Generic;
using LambdaBT;
using LambdaBT.Composite;
using LambdaBT.Decorator;
using NPCs.Enemies.Behaviour_Tree_Nodes;

namespace NPCs.Enemies
{
    public class GoblinBehaviourTree : LambdaBehaviourTree
    {
        public override LambdaBtNode ConstructBehaviourTree()
        {
            return new LambdaBtSequence(new List<LambdaBtNode>()
            {
                new LambdaBtRepeater(new IsPlayerInRange())
            });
        }
    }
}