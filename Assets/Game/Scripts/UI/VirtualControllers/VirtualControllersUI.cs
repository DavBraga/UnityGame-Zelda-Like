using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualControllersUI : MonoBehaviour
{
    [Header("Output")]
        public PlayerAvatar player;
    
    [SerializeField] GameObject interactionButton;
    [SerializeField]InteractionSystem interactionSystem;

    private void OnEnable() {
        interactionSystem.onANewInteractingObject+=ToggleOnInteractingButton;
        interactionSystem.onClearInteractingObject+=ToggleOffInteractingButton;

    }

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        player.SetMovmentVector(virtualMoveDirection);
    }

    public void VirtualLookInput(Vector2 virtualLookDirection)
    {
       // starterAssetsInputs.LookInput(virtualLookDirection);
    }

    public void VirtualJumpInput(bool virtualJumpState)
    {
        player.TryJump(virtualJumpState);
    }

    public void VirtualAttackInput(bool attack)
    {
        player.TryAttack(attack);
    }
    public void VirtualInteractInput(bool attack)
    {
        player.TryToInteract();
    }
    public void VirtualPlaceBombInput(bool attack)
    {
        player.TryUseTool();
    }

    public void VirtualUsePotionInput(bool attack)
    {
        player.TryUsePotion();
    }
    public void VirtualGuardInput(bool guard)
    {
        player.TryBlock(guard);
    }

    public void ToggleOnInteractingButton()
    {
        interactionButton.SetActive(true);
    }
    public void ToggleOffInteractingButton()
    {
        interactionButton.SetActive(false);
    }
        
    
}

