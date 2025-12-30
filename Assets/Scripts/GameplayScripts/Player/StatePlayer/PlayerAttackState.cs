using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(PlayerController _player) : base(_player) { }
    private float nextTimeToFire = 0f;
    public override void Enter()
    {
        player.StartScanning();
    }
    public override void Tick()
    {
        Transform target = player.currentTarget;
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            player.ChangeState(new PlayerIdleState(player));
            return;
        }
        if(Time.time > nextTimeToFire)
        {
            player.TriggerAttack();
            player.AttackSpeed();
            player.Shoot(target);
            nextTimeToFire = Time.time + 1f / player.currentAttackSpeed;
        }
    }
    public override void Exit() 
    {
        player.StopScanning();
    }
}
