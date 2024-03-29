using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{
    private UniversalAIProperties universalAIProperties;
    private ChaseAIProperties chaseAIProperties;
    private MonsterControllerAI monster;

    public ChaseState(MonsterControllerAI controller, UniversalAIProperties universalAIProperties, ChaseAIProperties chaseAIProperties)
    {
        this.universalAIProperties = universalAIProperties;
        this.chaseAIProperties =     chaseAIProperties;
        monster = controller;
        stateID = FSMStateID.Chase;
    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        universalAIProperties.host.Chasing = true;
    }

    public override void Reason(Transform player, Transform npc)
    {
        Debug.Log(npc.gameObject.name + " is chasing " + player.gameObject.name);

        if (!universalAIProperties.seeThroughWalls && universalAIProperties.host.WallCheck())
        {
            monster.PerformTransition(Transition.LoseSight);
            return;
        }

        if (IsInCurrentRange(npc, player.position, universalAIProperties.engagementDistance))
        {
            monster.PerformTransition(Transition.Reach);
            return;
        }

        if (chaseAIProperties.despawnDistance != -1 && (player.position - npc.position).magnitude > chaseAIProperties.despawnDistance)
        {
            Spawner.enemKilled();
            npc.gameObject.SetActive(false);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //Acting in this state is handled by Monster.cs
    }
}
