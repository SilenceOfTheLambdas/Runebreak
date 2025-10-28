namespace LambdaBT.Decorator
{
    public class LambdaBtRepeater : LambdaBtNode
    {
        private readonly LambdaBtNode _child;

        public LambdaBtRepeater(LambdaBtNode child) => _child = child;

        public override void Init()
        {
            _child.Init();
        }

        public override Result ExecuteFrame(float deltaTime)
        {
            _child.ExecuteFrame(deltaTime);
            return Result.Running;
        }

        public override Result ExecutePhysics(float fixedDeltaTime)
        {
            _child.ExecutePhysics(fixedDeltaTime);
            return Result.Running;
        }
    }
}