namespace LambdaBT.Decorator
{
    public class LambdaBtInverter : LambdaBtNode
    {
        private readonly LambdaBtNode _node;

        public LambdaBtInverter(LambdaBtNode node) => _node = node;

        public override Result ExecuteFrame(float deltaTime, LambdaBlackboard blackboard)
        {
            return _node.ExecuteFrame(deltaTime, blackboard) switch
            {
                Result.Running => Result.Running,
                Result.Failure => Result.Failure,
                Result.Success => Result.Success,
                _ => Result.Failure
            };
        }

        public override Result ExecutePhysics(float fixedDeltaTime, LambdaBlackboard blackboard)
        {
            return _node.ExecutePhysics(fixedDeltaTime, blackboard) switch
            {
                Result.Running => Result.Running,
                Result.Failure => Result.Failure,
                Result.Success => Result.Success,
                _ => Result.Failure
            };
        }
    }
}