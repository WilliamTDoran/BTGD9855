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

        if ((Player.plr.Rb.position - universalAIProperties.host.Rb.position).magnitude > (universalAIProperties.engagementDistance * 1.5f))
        {
            monster.PerformTransition(Transition.TooFar);
            return;
        }

        if (!Player.plr.Visible)
        {
            monster.PerformTransition(Transition.TooFar);
            return;
        }

        if (universalAIProperties.host.WallCheck())
        {
            monster.PerformTransition(Transition.TooFar);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        universalAIProperties.host.Aggress(universalAIProperties.engagementDistance, attackAIProperties.attackRange, attackAIProperties.avoidanceRange, attackAIProperties.avoidanceMask);
    }
}
