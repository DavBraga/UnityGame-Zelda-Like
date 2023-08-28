using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
   [SerializeField] List<PlayerAttack> attackChain; 

    [SerializeField] GameObject attackCollider;
    [SerializeField] GameObject shieldCollider;
    PlayerAvatar player;
    ShieldBlock shieldBlock;
    Health health;

    int powerModifer = 0;

    int CurrentAttackStage=0;

    

    private void Awake() {
        health = GetComponent<Health>();
        player = GetComponent<PlayerAvatar>();
        shieldBlock = GetComponent<ShieldBlock>();
    }

    private void OnEnable() 
    {
       GetComponent<PlayerDeath>().onTakeDamage+=TakeDamage;
         player.onAttack+= KeepChooping;
         player.onDefend+= Defend;
         player.onPowerIncrease+= increasePower;
         player.onStateInitializationFinished+=SetStates;
         player.onCombatPushed+=BePushed;
        
    }
    private void OnDisable() 
    {
        GetComponent<PlayerDeath>().onTakeDamage-=TakeDamage;
        player.onAttack-=KeepChooping;
        player.onDefend-= Defend;
        player.onPowerIncrease-= increasePower;
        player.onStateInitializationFinished-=SetStates;
        player.onCombatPushed-=BePushed;
        
    }
    private void Start()
    {
        SetUpWeaponColliders();
    }

    private void SetStates()
    {
        Debug.Log("states set");
        player.attackState.SetAttackChain(attackChain, attackCollider);
        player.defendState.SetShieldCollider(shieldCollider);
    }

    public void KeepChooping()
    {   
        if(player.exitiAttackTime>Time.time) return;
        if(player.stateMachine.currentState!=player.attackState)
            player.stateMachine.ChangeState(player.attackState);
    }
    public void Defend()
    {
        if(player.stateMachine.currentState!=player.defendState)
            player.stateMachine.ChangeState(player.defendState);
    }
    public bool TakeDamage(GameObject attacker, int damage)
    {
        if(player.stateMachine.currentState == player.deadState) return false;
        if(player.stateMachine.currentState== player.defendState&& !shieldBlock.DirectionCanDealDamage(attacker)) return false;
        
        if(!health.TakeDamage(damage)) return false;
            player.stateMachine.ChangeState(player.hurtState);   
        return true;
    }

     public void BePushed(GameObject pusher ,float pushPower, Vector3 direction)
     {
        if(player.stateMachine.currentState == player.deadState) return;
        if(player.stateMachine.currentState== player.defendState&& !shieldBlock.DirectionCanDealDamage(pusher)) return ;
        player.onPushed(pushPower,direction);

     }

    public void AttackTrigger(Collider other)
    {
        if(other.TryGetComponent(out CreatureController creatureController))
        {
            creatureController.TakeDamage(gameObject, attackChain[CurrentAttackStage].GetAttackStats().attackPower+powerModifer);
        }
        if(other.TryGetComponent(out Pushable pushable))
        {
            var positionDiff = other.gameObject.transform.position - transform.position;
            positionDiff.Normalize();
            player.onPushed.Invoke(attackChain[CurrentAttackStage].GetAttackStats().attaclknockbackPower,positionDiff);
        
        
        }
    }

    public void ShieldTrigger(Collider other)
    {
        if(other.TryGetComponent(out Pushable pushable))
        {
            var positionDiff = other.gameObject.transform.position - transform.position;
            positionDiff.Normalize();
            // use shield knocback instead
            pushable.BePushed(shieldBlock.ShieldKnocBack,positionDiff);
        }
    }

    public void PlayAttackImpulse(int attackStage)
    {
        player.onPlayerImpulse.Invoke(3f);
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
