using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileParams")]
public class ProjectileParams : ScriptableObject
{
    [SerializeField] private float speed = 200;
    [SerializeField] private int damage = 50;

    public float Speed { get { return speed; } }
    public int Damage { get { return damage; } }
}
