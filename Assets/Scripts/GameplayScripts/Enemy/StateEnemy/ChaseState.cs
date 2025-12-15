using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyController _enemy) : base(_enemy) { }
    public override void Enter()
    {
        Debug.Log("Dang duoi");
    }

    public override void Tick()
    {
        enemy.MoveTowardsPlayer();
        if(enemy.IsPlayerInRangeAttack())
        {
            enemy.ChangeState(new AttackState(enemy));
        }
        if (!enemy.IsPlayerInSight())
        {
            enemy.ChangeState(new IdleState(enemy));
        }

    }

    public override void Exit()
    {

    }
}
