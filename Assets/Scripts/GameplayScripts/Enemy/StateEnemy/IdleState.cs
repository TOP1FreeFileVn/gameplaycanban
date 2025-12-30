using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(EnemyController _enemy) :base (_enemy){}
    public override void Enter()
    {

    }

    public override void Tick()
    {

        if (enemy.IsPlayerInSight())
        {
            enemy.ChangeState(new ChaseState(enemy));

        }
    }
    
    public override void Exit()
    {

    }
}
