using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(EnemyController _enemy) : base(_enemy) { }
    private float nextTimeToAttack = 0f;
    public override void Enter()
    {
        enemy.OffBoolRun();
    }

    public override void Tick()
    {
        if (!enemy.IsPlayerInSight())
        {
            enemy.ChangeState(new IdleState(enemy));
        }
        else if (enemy.IsPlayerOutRangeAttack())
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
        if(Time.time > nextTimeToAttack)
        {
            enemy.TriggerAttack();
            nextTimeToAttack = Time.time + 1f / enemy.currentAttackSpeed;
        }

    }

    public override void Exit()
    {

    }
}
