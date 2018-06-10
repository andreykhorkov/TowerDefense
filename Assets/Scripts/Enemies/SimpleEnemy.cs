using UnityEngine;

public class SimpleEnemy : Enemy
{
    public override void Move()
    {
        transform.position += Vector3.forward * Time.deltaTime;
    }
}
