using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(EnemyController _enemy) : base(_enemy) { }
    public override void Enter()
    {
        Debug.Log("Dang bam");
    }

    public override void Tick()
    {
        enemy.MoveTowardsPlayer();
        if (!enemy.IsPlayerInSight())
        {
            enemy.ChangeState(new IdleState(enemy));
        }
        else if (enemy.IsPlayerOutRangeAttack())
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
       

    }

    public override void Exit()
    {

    }
}
