using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //player.SetVelocity(xInput*player.moveSpeed, rb.velocity.y);
        if (player.isWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        
        if (player.isGroundDetected())
            stateMachine.ChangeState(player.idleState);
        if(xInput!=0)
            player.SetVelocity(xInput * player.moveSpeed*0.5f, rb.velocity.y);

    }
}
