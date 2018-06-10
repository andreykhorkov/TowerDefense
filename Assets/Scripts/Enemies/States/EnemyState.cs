namespace Enemies
{
    public abstract class EnemyState
    {
        protected Enemy enemy;

        protected EnemyState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public abstract void OnStateEntered();
        public abstract void OnStateLeave();
        public abstract void Move();
    }
}
