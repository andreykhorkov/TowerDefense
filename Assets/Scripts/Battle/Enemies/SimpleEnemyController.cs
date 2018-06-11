public class SimpleEnemyController : EnemyController
{
    public override string VehicleName
    {
        get { return "Tank"; }
    }

    public override EnemyType EnemyType
    {
        get { return EnemyType.simple; }
    }
}
