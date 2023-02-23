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
        if (universalAIProperties.seeThroughWalls && Player.plr.Visible)
        {
            monster.PerformTransition(Transition.Spot);
        }

        if (!universalAIProperties.host.WallCheck() && Player.plr.Visible)
        {
            monster.PerformTransition(Transition.Spot);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        
    }
}
