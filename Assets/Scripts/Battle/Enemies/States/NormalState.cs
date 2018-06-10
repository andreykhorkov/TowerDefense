namespace Enemies
{
    public class NormalState : EnemyState
    {
        public NormalState(EnemyController enemy) : base(enemy)
        {
        }

        public override void OnStateEntered()
        {
           
        }

        public override void OnStateLeave()
        {

        }

        public override void Move()
        {
            enemy.Move();
        }
    }
}
