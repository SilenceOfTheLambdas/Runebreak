namespace LambdaBT.Decorator
{
    public class LambdaBtRepeater : LambdaBtNode
    {
        private readonly LambdaBtNode _child;

        public LambdaBtRepeater(LambdaBtNode child) => _child = child;

        public override Result ExecuteFrame(float deltaTime, LambdaBlackboard blackboard)
        {
            _child.ExecuteFrame(deltaTime, blackboard);
            return Result.Running;
        }

        public override Result ExecutePhysics(float fixedDeltaTime, LambdaBlackboard blackboard)
        {
            _child.ExecutePhysics(fixedDeltaTime, blackboard);
            return Result.Running;
        }
    }
}