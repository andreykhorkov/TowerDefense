using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileParams")]
public class ProjectileParams : ScriptableObject
{
    [SerializeField] private float speed = 200;

    public float Speed { get { return speed; } }
}
