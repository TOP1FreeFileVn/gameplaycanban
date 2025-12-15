using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState 
{
    public PlayerIdleState(PlayerController _player) : base(_player) { }
    public override void Enter()
    {
        player.StartScanning();
    }
    public override void Tick()
    {
        Transform target = player.currentTarget;
        if (target != null && target.gameObject.activeInHierarchy)
        {
            player.ChangeState(new PlayerAttackState(player));
            return;
        }
    }
    public override void Exit()
    {
        player.StopScanning();
    }
}
