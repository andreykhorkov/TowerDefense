using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParams")]
public class EnemyParams : ScriptableObject
{
    [SerializeField] private float defaultSpeed;

    public float DefaultSpeed { get { return defaultSpeed; } }
}
