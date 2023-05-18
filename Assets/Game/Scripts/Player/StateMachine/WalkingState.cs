using UnityEngine;
public class WalkingState : State
{
    PlayerController player;
    public WalkingState(PlayerController playerController) : base("Walking")
    {
        player = playerController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        if(!player.isGrounded())
            player.stateMachine.ChangeState(player.onAirState);

        if(player.inputMovmentVector.isZero())
        {
            player.stateMachine.ChangeState(player.idleState);
        }  
        if(player.ReadJumpInput()) player.Jump();
        base.OnStateUpdate();
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();    
        player.PlayerMovment();
        player.RotateBodyToFace();
    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}