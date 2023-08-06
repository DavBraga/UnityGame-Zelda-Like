using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateRedone : State
{
    PlayerController player;

    GameObject attackCollider;
    int attackStage = -1; 

    float attackChainWindow = .3f;

    float startTime;
    //float DurationTime;
    float anticipationTime;
    float attackCoolDown;
    bool applyImpulse = false;

    float effectDuration = 0.6f;
    bool isEffectsActive = false;
    public AttackStateRedone(PlayerController playerController, GameObject attackCollider) : base("Attack")
    {
        player = playerController;
        this.attackCollider = attackCollider;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        player.animator.SetBool("bIsAttacking", true);
        attackStage = 0;
        StartAttack();
        SetAttackTimers();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
       attackCollider.SetActive(false);
       player.animator.SetBool("bIsAttacking", false);
       player.exitiAttackTime = Time.time+attackCoolDown;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        // se excedeu a duração voltar para idle;
        if(Time.time>startTime+anticipationTime+effectDuration+attackCoolDown+attackChainWindow){
            player.stateMachine.ChangeState(player.idleState);
            return;
        }
        // espera antencipação
        if(Time.time<startTime+anticipationTime) return;
        // aplicar efeitos após tempo de antecipação
        if(Time.time< startTime+ anticipationTime+effectDuration)
        if(!isEffectsActive) TriggerAttackEffects(attackStage);

        // espera o tempo de efeito para desliga-lo e ler inputs
        if(Time.time> startTime+ anticipationTime+effectDuration)
        {
            // destivar efeitos se o tempo de efeito ja passou
            if(isEffectsActive)
            {
                attackCollider.SetActive(false);
                isEffectsActive = false;
            }
            //cancelar se jogador se movimenta
            if(!player.ReadAttackInput())
            {
                player.stateMachine.ChangeState(player.idleState);
                return;
            }
        }
        // após o cooldown abrir input do jogador
         if(Time.time> startTime+ anticipationTime+effectDuration+attackCoolDown)
            { 
           
                // abrir janela de encadeamento de ataques
                if (player.ReadAttackInput())
                {
                    EvolveAttackStages();
                    StartAttack();
                    return;
                } 
                return;
            }
    }

    private void StartAttack()
    {
        player.PlayAttackAnimation(attackStage);
        
       // player.PlayAttackImpulse(attackStage);
       
        // play sound
        player.mySFXManager.PlayAudio();
        SetAttackTimers();
    }

    public void TriggerAttackEffects(int stage)
    {
        
        isEffectsActive = true;
        //turn on colliders
        attackCollider.SetActive(true);
        applyImpulse = true;
        //add impulse
        
        
        
    }
    public void EvolveAttackStages()
    {
        attackStage++;
        Debug.Log("went attack stage:"+attackStage);
        // trocar 2 por uma variável.
        if (attackStage > 2) attackStage = 0;
        player.attackstage = attackStage;
    }

    private void SetAttackTimers()
    {
        startTime = Time.time;
        anticipationTime = player.GetAttackPreparationTIme()[attackStage];
        effectDuration = player.attackDuration[attackStage];
        attackCoolDown = player.attackCooldown[attackStage];
        
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        player.RotateBodyToFace(1);
        if(applyImpulse) 
        {
            player.PlayAttackImpulse(attackStage);
            applyImpulse = false;
        }
        // if(Time.time< startTime+ anticipationTime+effectDuration)
        // player.PlayerMovment(.3f);
    }   
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}

// start attack
// espera tempo de execução
// aplicar efeitos
//abre janela de chain