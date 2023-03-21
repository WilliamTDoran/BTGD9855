using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
    private UniversalAIProperties universalAIProperties;
    private PatrolAIProperties patrolAIProperties;
    private MonsterControllerAI monster;

    public PatrolState(MonsterControllerAI controller, UniversalAIProperties universalAIProperties, PatrolAIProperties patrolAIProperties)
    {
        this.universalAIProperties = universalAIProperties;
        this.patrolAIProperties =    patrolAIProperties;
        monster = controller;
        stateID = FSMStateID.Patrol;
    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        universalAIProperties.host.Chasing = false;
    }

    public override void Reason(Transform player, Transform npc)
    {
        if (Player.plr.Visible && universalAIProperties.seeThroughWalls)
        {
            monster.PerformTransition(Transition.Spot);
            return;
        }

        if (Player.plr.Visible && !universalAIProperties.host.WallCheck() && ((Player.plr.Rb.position - universalAIProperties.host.Rb.position).magnitude < patrolAIProperties.spottyDistance))
        {
            monster.PerformTransition(Transition.Spot);
            return;
        }

        if (patrolAIProperties.despawnDistance != -1 && (player.position - npc.position).magnitude > patrolAIProperties.despawnDistance)
        {
            Spawner.enemKilled();
            npc.gameObject.SetActive(false);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        
    }
}
