namespace Projectiles
{
    public class ThrownState : ProjectileState
    {
        public ThrownState(Projectile projectile) : base(projectile)
        {
        }

        public override void OnStateEntered()
        {
           
        }

        public override void OnStateLeave()
        {

        }

        public override void Update()
        {
            projectile.Move();
        }

        
    }
}
