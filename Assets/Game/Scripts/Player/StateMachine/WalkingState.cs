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
        if(!player.GetControlledAvatar().isGroundedDelegate())
            player.StateMachine.ChangeState(player.OnAirState);

        if(player.inputMovmentVector.isZero())
        {
            player.StateMachine.ChangeState(player.IdleState);
        }  
        base.OnStateUpdate();
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();    
        player.GetControlledAvatar().onMove.Invoke(1);
        player.GetControlledAvatar().onRotate.Invoke(.15f);
        
    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}