using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : FSMState
{
    private UniversalAIProperties universalAIProperties;
    private AttackAIProperties attackAIProperties;
    private MonsterControllerAI monster;

    public AttackState(MonsterControllerAI controller, UniversalAIProperties universalAIProperties, AttackAIProperties attackAIProperties)
    {
        this.universalAIProperties = universalAIProperties;
        this.attackAIProperties =    attackAIProperties;
        monster = controller;
        stateID = FSMStateID.Attack;
    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        universalAIProperties.host.Chasing = false;
    }

    public override void Reason(Transform player, Transform npc)
    {
        Debug.Log(npc.gameObject.name + " is attacking " + player.gameObject.name);

        if (true)
        {
            monster.PerformTransition(Transition.TooFar);
        }


    }

    public override void Act(Transform player, Transform npc)
    {
        if (true)
        {
            
        }
    }
}
