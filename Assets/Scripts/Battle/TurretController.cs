using Battle;
using UnityEngine;

public class TurretController : BattleElement
{

    [SerializeField] private Transform cannon;
    [SerializeField, Tooltip("degrees/sec")] private float maxTurretRotationSpeed = 360;

    public Transform target;

    private Vector3 dirr;

    void Update()
    {
        var dir = (target.position - cannon.position).normalized;
        dirr = dir;

        AimTurret(dir);
        AimCannon(dir); 
    }

    private void AimTurret(Vector3 dir)
    {
        var isEnemyInFront = Vector3.Dot(dir, transform.forward) > 0;
        var dotProd = Vector3.Dot(dir, transform.right);
        float engineValue;

        if (isEnemyInFront)
        {
            engineValue = dotProd;
        }
        else
        {
            engineValue = dotProd > 0 ? 1 : -1;
        }

        transform.rotation *= Quaternion.AngleAxis(engineValue * maxTurretRotationSpeed * Time.deltaTime, transform.up);
    }

    private void AimCannon(Vector3 dir)
    {
        var projection = Vector3.ProjectOnPlane(dir, transform.right);
        cannon.transform.rotation *= Quaternion.AngleAxis(Vector3.Dot(projection, -cannon.transform.up) * maxTurretRotationSpeed * Time.deltaTime, Vector3.right);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cannon.position, cannon.transform.forward*100);
    }
}
