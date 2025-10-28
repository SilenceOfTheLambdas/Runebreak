using System;
using System.Collections.Generic;
using LambdaBT;
using LambdaBT.Composite;
using LambdaBT.Decorator;
using NPCs.Enemies.Behaviour_Tree_Nodes;
using UnityEngine;

namespace NPCs.Enemies
{
    public class GoblinBehaviourTree : LambdaBehaviourTree
    {
        public override LambdaBtNode ConstructBehaviourTree()
        {
            return new LambdaBtSequence(new List<LambdaBtNode>()
            {
                new LambdaBtSequence(new List<LambdaBtNode>()
                {
                    // new LambdaBtInverter(new IsOnLedge()),
                    new IsPlayerInRange(this),
                }),
                new ChasePlayer(this)
            });
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, (float) blackboard.GetValueByName("ChaseRadius"));
        }
    }
}