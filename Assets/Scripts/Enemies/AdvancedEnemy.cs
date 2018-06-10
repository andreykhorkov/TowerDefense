using UnityEngine;

public class AdvancedEnemy : Enemy
{
    public override void Move()
    {
        transform.position += Vector3.forward * Time.deltaTime;
    }
}
