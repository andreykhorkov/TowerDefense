using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParams")]
public class EnemyParams : ScriptableObject
{
    [SerializeField] private float defaultSpeed;
    [SerializeField] private int defaultHealth;

    public float DefaultSpeed { get { return defaultSpeed; } }
    public int DefaultHealth { get { return defaultHealth; } }
}
