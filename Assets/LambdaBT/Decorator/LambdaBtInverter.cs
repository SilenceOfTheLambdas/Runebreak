namespace LambdaBT.Decorator
{
    public class LambdaBtInverter : LambdaBtNode
    {
        private readonly LambdaBtNode _node;

        public LambdaBtInverter(LambdaBtNode node) => _node = node;

        public override void Init()
        {
            
        }

        public override Result ExecuteFrame(float deltaTime)
        {
            return _node.ExecuteFrame(deltaTime) switch
            {
                Result.Running => Result.Running,
                Result.Failure => Result.Failure,
                Result.Success => Result.Success,
                _ => Result.Failure
            };
        }

        public override Result ExecutePhysics(float fixedDeltaTime)
        {
            return _node.ExecutePhysics(fixedDeltaTime) switch
            {
                Result.Running => Result.Running,
                Result.Failure => Result.Failure,
                Result.Success => Result.Success,
                _ => Result.Failure
            };
        }
    }
}