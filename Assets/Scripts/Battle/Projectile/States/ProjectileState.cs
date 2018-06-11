namespace Projectiles
{
    public abstract class ProjectileState
    {
        protected Projectile projectile;

        protected ProjectileState(Projectile projectile)
        {
            this.projectile = projectile;
        }

        public abstract void OnStateEntered();
        public abstract void OnStateLeave();
        public abstract void Update();
    }
}
