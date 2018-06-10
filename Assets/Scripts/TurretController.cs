using UnityEngine;

public class TurretController : MonoBehaviour
{

    [SerializeField] private Transform cannon;
    [SerializeField, Tooltip("degrees/sec")] private float maxTurretRotationSpeed = 360;

    private Vector3 turretPosition;

    public Transform target;

    private void Start()
    {
        turretPosition = transform.position;
    }

    void Update()
    {
        var dir = (target.position - turretPosition).normalized;

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

        transform.rotation *= Quaternion.AngleAxis(engineValue * maxTurretRotationSpeed * Time.deltaTime, Vector3.up);
    }

    private void AimCannon(Vector3 dir)
    {
        var projection = Vector3.ProjectOnPlane(dir, transform.right);
        cannon.transform.rotation *= Quaternion.AngleAxis(Vector3.Dot(projection, -cannon.transform.up) * maxTurretRotationSpeed * Time.deltaTime, Vector3.right);
    }
}
