public class AdvancedEnemyController : EnemyController
{
    public override string VehicleName
    {
        get { return "Car"; }
    }

    public override EnemyType EnemyType
    {
        get { return EnemyType.advanced; }
    }
}
