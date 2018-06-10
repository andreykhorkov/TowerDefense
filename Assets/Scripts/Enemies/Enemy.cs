using Enemies;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected EnemyState idleState;
    protected EnemyState normalState;
    protected EnemyState currentState;

    private void Start()
    {
        idleState = new IdleState(this);
        normalState = new NormalState(this);
        currentState = normalState;
    }

    public abstract void Move();

    public virtual void Update()
    {
        currentState.Move();
    }
}
