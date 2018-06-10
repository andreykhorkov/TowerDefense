namespace Enemies
{
    public abstract class EnemyState
    {
        protected EnemyController enemy;

        protected EnemyState(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public abstract void OnStateEntered();
        public abstract void OnStateLeave();
        public abstract void Move();
    }
}
