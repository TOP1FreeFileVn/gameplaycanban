using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState 
{
    protected EnemyController enemy;

    public EnemyState(EnemyController _enemy)
    {
        this.enemy = _enemy; 
    }
    public abstract void Enter();

    public abstract void Tick();

    public abstract void Exit();
}
