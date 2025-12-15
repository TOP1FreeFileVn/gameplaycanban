using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState 
{
    protected PlayerController player;
    public PlayerState(PlayerController _player)
    {
        this.player = _player; 
    }
    public abstract void Enter();

    public abstract void Tick();

    public abstract void Exit();
}
