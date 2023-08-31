using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
   [SerializeField] List<PlayerAttack> attackChain; 

    [SerializeField] GameObject attackCollider;
    [SerializeField] GameObject shieldCollider;
    PlayerController player;
    PlayerAvatar avatar;
    ShieldBlock shieldBlock;
    Health health;

    int powerModifer = 0;

    int CurrentAttackStage=0;
    private void Awake() {
        health = GetComponent<Health>();
        player = GetComponent<PlayerController>();
        avatar = player.GetControlledAvatar();
        shieldBlock = avatar.GetComponent<ShieldBlock>();
    }

    private void OnEnable() 
    {
       GetComponent<PlayerLifeCycle>().onTakeDamage+=TakeDamage;
         player.onAttack+= KeepChooping;
         player.onDefend+= Defend;
         player.onPowerIncrease+= increasePower;
         player.onStateInitializationFinished+=SetStates;
         avatar.onCombatPushed+=BePushed;
        
    }
    private void OnDisable() 
    {
        GetComponent<PlayerLifeCycle>().onTakeDamage-=TakeDamage;
        player.onAttack-=KeepChooping;
        player.onDefend-= Defend;
        player.onPowerIncrease-= increasePower;
        player.onStateInitializationFinished-=SetStates;
        avatar.onCombatPushed-=BePushed;
        
    }
    private void Start()
    {
        SetUpWeaponColliders();
    }

    private void SetStates()
    {
        Debug.Log("states set");
        player.AttackState.SetAttackChain(attackChain, attackCollider);
        player.DefendState.SetShieldCollider(shieldCollider);
    }

    public void KeepChooping()
    {   
        if(player.exitiAttackTime>Time.time) return;
        if(player.StateMachine.currentState!=player.AttackState)
            player.StateMachine.ChangeState(player.AttackState);
    }
    public void Defend()
    {
        if(player.StateMachine.currentState!=player.DefendState)
            player.StateMachine.ChangeState(player.DefendState);
    }
    public bool TakeDamage(GameObject attacker, int damage)
    {
        if(player.StateMachine.currentState == player.DeadState) return false;
        if(player.StateMachine.currentState== player.DefendState&& !shieldBlock.DirectionCanDealDamage(attacker)) return false;
        
        if(!health.TakeDamage(damage)) return false;
            player.StateMachine.ChangeState(player.HurtState);   
        return true;
    }

     public void BePushed(GameObject pusher ,float pushPower, Vector3 direction)
     {
        if(player.StateMachine.currentState == player.DeadState) return;
        if(player.StateMachine.currentState== player.DefendState&& !shieldBlock.DirectionCanDealDamage(pusher)) return ;
        avatar.onPushed(pushPower,direction);

     }

    public void AttackTrigger(Collider other)
    {
        if(other.TryGetComponent(out CreatureController creatureController))
        {
            creatureController.TakeDamage(gameObject, attackChain[CurrentAttackStage].GetAttackStats().attackPower+powerModifer);
        }
        if(other.TryGetComponent(out Pushable pushable))
        {
            var positionDiff = other.transform.position - avatar.transform.position;
            Debug.Log(positionDiff);
            positionDiff.Normalize();
            pushable.BePushed(attackChain[CurrentAttackStage].GetAttackStats().attackknockbackPower,positionDiff);
        
        
        }
    }

    public void ShieldTrigger(Collider other)
    {
        if(other.TryGetComponent(out Pushable pushable))
        {
            var positionDiff = other.transform.position - avatar.transform.position;
            positionDiff.Normalize();
            // use shield knocback instead
            pushable.BePushed(shieldBlock.ShieldKnocBack,positionDiff);
        }
    }

    public void PlayAttackImpulse(int attackStage)
    {
        avatar.onPlayerImpulse.Invoke(3f);
    }

    private void SetUpWeaponColliders()
    {
        attackCollider.GetComponent<WeaponCollision>().onHit += AttackTrigger;
        shieldCollider.GetComponent<WeaponCollision>().onHit += ShieldTrigger;
        attackCollider.SetActive(false);
        shieldCollider.SetActive(false);
    }
    public void increasePower()
    {
        powerModifer++;
    }

}
